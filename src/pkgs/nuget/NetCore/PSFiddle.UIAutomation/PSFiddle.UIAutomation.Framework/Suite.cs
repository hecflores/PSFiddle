
using MC.Track.Shared;
using PSFiddle.UIAutomation.Framework.Shared;
using MC.Track.TestSuite.Driver;
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Dependencies.Builders;
using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Factories;
using MC.Track.TestSuite.Interfaces.Pages;
using MC.Track.TestSuite.Interfaces.Pages.Shared;
using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Interfaces.Repositories;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Services.Functional;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Interfaces.Instructions;
using MC.Track.TestSuite.Interfaces.Instructions.Shared;
using MC.Track.TestSuite.Toolkit.Instructions;
using MC.Track.TestSuite.Toolkit.Instructions.Shared;
using MC.Track.TestSuite.Model.Dto;
using MC.Track.TestSuite.Model.Enums;
using MC.Track.TestSuite.Model.EventArgs;
using MC.Track.TestSuite.Model.Helpers;
using MC.Track.TestSuite.Model.Types;
using MC.Track.TestSuite.Proxies;
using MC.Track.TestSuite.Repository;
using MC.Track.TestSuite.Services.Factories;
using MC.Track.TestSuite.Services.Services;
using MC.Track.TestSuite.Services.Util;
using MC.Track.TestSuite.Toolkit.Dependencies;
using MC.Track.TestSuite.Toolkit.Dependencies.Builders;
using MC.Track.TestSuite.Toolkit.Pages.Shared;
using MC.Track.TestSuite.UI.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Injection;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.Interceptors;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;
using Unity.Lifetime;
using Unity.Registration;
using System.Text.RegularExpressions;

namespace MC.Track.TestSuite.Toolkit
{
    

    public class Suite : IResolver
    {
        private List<IInterceptionBehavior> Interceptors { get; set; }
        private List<KeyValuePair<Type, AthenaInterceptor>> RegisteredInterceptors { get; set; }
        private IConfiguration config;
        private Func<String, Object> getAction;
        private Action<String, Object> setAction;
        private Action<String> saveFileAction;
        

        public Suite(IConfiguration config, Func<String, Object> getAction,Action<String, Object> setAction, Action<String> saveFileAction)
        {
            this.Interceptors = new List<IInterceptionBehavior>();
            this.config = config;
            this.getAction = getAction;
            this.setAction = setAction;
            this.saveFileAction = saveFileAction;
            UpdateContainer();
        }
        
        public IPages Pages { get => this.Resolve<IPages>(); }
        public IDependencies Dependencies { get => this.Resolve<IDependencies>(); }
        public IStateManagment State { get => this.Resolve<IStateManagment>(); }
        
        public IBrowserDependencies Browsers { get => this.Dependencies.Browser(); }

        #region Base Routines
        private IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();
            container.AddNewExtension<Interception>();
            return container;
        }

        private static bool IsNET45 = Type.GetType("System.Reflection.ReflectionContext", false) != null;


        public IFunctionProxyEnabler<T> InterceptClass<T>()
        {
            return Resolve<IFunctionProxyEnablerFactory>().Create<T>();
        }
        public void SubscribeToResolver(Action<PreInvokedEventBasedProxyEventArgs> callback)
        {
            this.Resolve<IEventBasedProxy>().PreInvokedSubscription+=(sender, arg) => {
                callback(arg);
            };
        }
        public void RegisterAll(Assembly assembly)
        {
            var logger = this.Resolve<ILogger>();
            logger.LogTrace($"Registering Assembly {assembly.FullName}");

            // Get all interceptorTypes (Classes with the attribute given in A)
            try
            {
                assembly.GetTypes()
                    .Where(b => b.IsClass)
                    .Select(b => new KeyValuePair<Type, AthenaRegisterAttribute>(b, (AthenaRegisterAttribute)Attribute.GetCustomAttribute(b, typeof(AthenaRegisterAttribute))))
                    .Where(b => b.Value != null)
                    .ToList()
                    .ForEach((b) =>
                    {
                        b.Value.Register(this, b.Key);
                    });
            }
            catch (Exception e)
            {
                logger.LogError($"Unable to load assembly {assembly.FullName}");
                logger.LogError(e);
            }
        }
        private void RegisterAll()
        {
            
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                RegisterAll(a);
            }
        }
        private void RegisterInterceptors()
        {
            RegisteredInterceptors = new List<KeyValuePair<Type, AthenaInterceptor>>();
            //foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    // Get all interceptorTypes (Classes with the attribute given in A)
            //    a.GetTypes()
            //        .Where(b => b.IsClass)
            //        .Select(b => new KeyValuePair<Type, AthenaInterceptor>(b, (AthenaInterceptor)Attribute.GetCustomAttribute(b, typeof(AthenaInterceptor))))
            //        .Where(b => b.Value != null)
            //        .ToList()
            //        .ForEach((b) =>
            //        {
            //            RegisteredInterceptors.Add(b);
            //        });
            //}

            //// Register the actual interceptor types
            //RegisteredInterceptors.ForEach((item) =>
            //{
            //    this.Container.RegisterType(item.Key);
            //}); 


        }
        private List<InjectionMember> GetDefaultInjectionMembers()
        {
            

            var list = new List<InjectionMember>();// this.RegisteredInterceptors.Where(b => b.Value.CanIntercept<T>()).Select(b => new InterceptionBehavior(b.Key)).Cast<InjectionMember>().ToList();
            list.Add(new Interceptor<InterfaceInterceptor>()); //Interception technique
            return list;
        }
        private void Config()
        {
            this.RegisterInstance<IResolver>(this);
            this.RegisterSingleton<ISmartResourceDestroyerServiceFactory, SmartResourceDestroyerServiceFactory>();
            this.RegisterSingleton<IEventBasedProxy, EventBasedProxy>();
            this.UseInterceptor<IEventBasedProxy>();

            this.RegisterSingleton<IGenericScopingFactory, GenericScopingFactory>();
            this.RegisterSingleton<ILogger, Logger>();
            this.RegisterSingleton<IDisposableTracker, DisposableTracker>();
            
            

            // Add normal consoles.
            var logger = Resolve<ILogger>();
            XConsole.Clean();
            XConsole.ClearProxies();
            XConsole.Listen((text) => logger.LogTrace(text));
            logger.Subscribe((sender, text) =>
            {
                text = Regex.Replace(text, @"\*\*(.*)\*\*", "$1");
                text = Regex.Replace(text, @"_(.*)_", "$1");
                Console.WriteLine(text);
            }).DistroyWithSuite();
            
            
            this.RegisterInstance<IConfiguration>(config);
            this.RegisterSingleton<ITrackBrowser, TrackBrowser>();
            this.RegisterType<ITestableTrackBrowser, TestableTrackBrowser>();
            this.RegisterSingleton<IElementDiscovery, ElementDiscovery>();
            this.RegisterSingleton<IFileManager, FileManager>();
            this.RegisterSingleton<IParameterParser, ParameterParser>();
            this.RegisterSingleton<IEmailService, EmailService>();
            this.RegisterSingleton<IMailMessageService, MailMessageService>();
            this.RegisterSingleton<ISqlInstanceService, SqlInstanceService>(new Object[] { this.Resolve<IConfiguration>().DatabaseConnectionString});
            
            this.RegisterSingleton<ITrackTestRunner, TrackTestRunner>();

            
            this.RegisterSingleton<IRawRepository, RawRepository>();
            
            this.RegisterSingleton<IDynamicUserFactoryService, DynamicUserFactoryService>(new Object[] { this.Resolve<IConfiguration>().TestUsersXML });
            
            //this.RegisterType<Type>("Browser", typeof(ITestableTrackBrowser));
            //this.RegisterType<IMagicFactory<ITestableTrackBrowser>, ChromeBrowserMagicFactory<TestableChromeBrowser>>("Chrome Browser");
            //this.RegisterType<IMagicFactory<ITestableTrackBrowser>, ChromeBrowserMagicFactory<TestableChromeBrowser>>("Chrome");
            //this.RegisterType<IMagicFactory<ITestableTrackBrowser>, ChromeBrowserMagicFactory<TestableChromeIncognitoBrowser>>("Incognito Chrome Browser");
            //this.RegisterType<IMagicFactory<ITestableTrackBrowser>, ChromeBrowserMagicFactory<TestableChromeIncognitoBrowser>>("Incognito Chrome");

            //this.RegisterType<Type>("User", typeof(UserType));
            //this.RegisterType<IMagicFactory<UserType>, UserAdminMagicFactory>("Track Admin");
            //this.RegisterType<IMagicFactory<UserType>, UserBuyerMagicFactory>("Buyer");
            //this.RegisterType<IMagicFactory<UserType>, UserSupplierMagicFactory>("Supplier");
            //this.RegisterType<IMagicFactory<UserType>, UserOnboardedMagicFactory>("Onboarded");
            //this.RegisterType<IMagicFactory<UserType>, UserNotOnboardedMagicFactory>("Unregistered");

            //this.RegisterType<Type>("Organization", typeof(OrganizationType));
            //this.RegisterType<IMagicFactory<OrganizationType>, OrganizationGenericMagicFactory>("Onboarded Organization");

            //this.RegisterType<Type>("StagingOrganization", typeof(StagingOrganizationType));
            //this.RegisterType<IMagicFactory<StagingOrganizationType>, StagingOrganizationNewMagicFactory>("New Organization");

            this.RegisterSingleton<IStateManagment, StateManagment>(new Object[] { getAction, setAction});
            this.RegisterSingleton<IFileSaverService, FileSaverService>(new Object[] { saveFileAction }); ;

            this.RegisterSingleton<IStorageService, StorageService>(StorageServiceTypes.CoreServices, new object[] { this.Resolve<IConfiguration>().CoreServicesStorageAccountConnectionString, this});
            this.RegisterSingleton<IStorageService, StorageService>(StorageServiceTypes.TestServices, new object[] { this.Resolve<IConfiguration>().TestServicesStorageAccountConnectionString, this});

            this.RegisterSingleton<ICloudStorageService, CloudStorageService>(StorageServiceTypes.CoreServices, new object[] { this.Resolve<IConfiguration>().CoreServicesStorageAccountConnectionString });
            this.RegisterSingleton<ICloudStorageService, CloudStorageService>(StorageServiceTypes.TestServices, new object[] { this.Resolve<IConfiguration>().TestServicesStorageAccountConnectionString });
            this.RegisterSingleton<Pages.Shared.Pages>();
            this.RegisterSingleton<IPages, Pages.Shared.Pages>();
            this.RegisterSingleton<IInstructions, Instructions.Shared.Instructions>();
            //this.RegisterSingleton<IHomePage, HomePage>();
            //this.RegisterSingleton<ILoginPage, LoginPage>();
            //this.RegisterSingleton<IMyBuyersPage, MyBuyersPage>();
            //this.RegisterSingleton<IMySuppliersPage, MySuppliersPage>();
            //this.RegisterSingleton<IOnboardingPage, OnboardingPage>();
            //this.RegisterSingleton<IPaymentServicesPage, PaymentServicesPage>();
            //this.RegisterSingleton<IRelationshipDetailsPage, RelationshipDetailsPage>();
            //this.RegisterSingleton<ISearchDirectoryResultsPage, SearchDirectoryResultsPage>();
            //this.RegisterSingleton<ISupplierInvitationsPage, SupplierInvitationsPage>();
            //this.RegisterSingleton<IUserAgreementPage, UserAgreementPage>();
            //this.RegisterSingleton<IUsersPage, UsersPage>();
            //this.RegisterSingleton<IAdminSearchDirectoryPage, AdminSearchDirectoryPage>();
            //this.RegisterSingleton<IBuyerInvitationsPage, BuyerInvitationsPage>();
            //this.RegisterSingleton<ICompanyProfilePage, CompanyProfilePage>();
            //this.RegisterSingleton<IAdminSearchDirectoryResultsPage, AdminSearchDirectoryResultsPage>();

            this.RegisterSingleton<IBrowsersBuilder, BrowsersBuilder>();
            this.RegisterSingleton<IDependencies, Dependencies.Dependencies>();
            this.RegisterSingleton<IBrowserDependencies, BrowserDependencies>();
            this.RegisterSingleton<IEmailDependencies, EmailDependencies>();
            this.Container.AddNewExtension<Interception>();
            this.RegisterAll();
            
            

           
        }
        
        private object _lock = new object();
        public IUnityContainer Container { get; private set; } = null;

        private void UpdateContainer()
        {
            lock (_lock)
            {
                if (Container == null)
                {
                    Container = CreateContainer();
                    Config();
                }
            }
        }
       


        public T Resolve<T>() where T:class
        {
            return this.Container.Resolve<T>();
        }
        public T Resolve<T>(string Name) where T : class
        {
            return this.Container.Resolve<T>(Name);
        }
        public void Dispose()
        {
            this.Resolve<IDisposableTracker>().Dispose();
            this.Container.Dispose();
        }

        public void RegisterType<TInstance>()
        {
            this.Container.RegisterType<TInstance>();
        }

        public void RegisterType<TInterface, TInstance>(params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.AddRange(injectionMembers);

            this.Container.RegisterType<TInterface, TInstance>(injectionMembers: (this.Interceptors.Select(b => new InterceptionBehavior(b)).ToArray()));
        }

        public void RegisterInstance<TInterface>(TInterface instance)
        {
            this.Container.RegisterInstance<TInterface>(instance);
        }

        public void RegisterSingleton<TInterface, TInstance>(params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.AddRange(injectionMembers);

            this.Container.RegisterSingleton<TInterface, TInstance>(injectionMembers: members.ToArray());
        }

        public void RegisterSingleton<TInterface, TInstance>(TInterface instance, params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.Add(new InjectionFactory((unity) => instance));
            members.AddRange(injectionMembers);

            this.Container.RegisterSingleton<TInterface, TInstance>(injectionMembers: members.ToArray());
        }

        public void RegisterType<TInterface, TInstance>(Func<IResolver, TInterface> factory, params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.Add(new InjectionFactory((unity) =>
            {
                return factory(this);
            }));
            members.AddRange(injectionMembers);


            this.Container.RegisterType<TInterface, TInstance>(injectionMembers: members.ToArray());
        }

        public void RegisterType<TInterface, TInstance>(object[] contructorParameters, params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.Add(new InjectionConstructor(contructorParameters));
            members.AddRange(injectionMembers);

            this.Container.RegisterType<TInterface, TInstance>(injectionMembers: members.ToArray());
        }

        public void RegisterSingleton<TInterface>(Func<IResolver, TInterface> factory, params InjectionMember[] injectionMembers)
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.Add(new InjectionFactory((unity) =>
            {
                return factory(this);
            }));
            members.AddRange(injectionMembers);

            this.Container.RegisterSingleton<TInterface>(injectionMembers: members.ToArray());
        }

        public void RegisterSingleton<TInterface, TInstance>(object[] contructorParameters, params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.Add(new InjectionConstructor(contructorParameters));
            members.AddRange(injectionMembers);

            this.Container.RegisterSingleton<TInterface, TInstance>(injectionMembers: members.ToArray());
        }

        public void RegisterType<TInterface, TInstance>(string Name, object[] contructorParameters, params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.Add(new InjectionConstructor(contructorParameters));
            members.AddRange(injectionMembers);

            this.Container.RegisterType<TInterface, TInstance>(Name, injectionMembers: members.ToArray());
        }

        public void RegisterType<TInterface, TInstance>(string Name, Func<IResolver, TInterface> factory, params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.Add(new InjectionFactory((unity) =>
            {
                return factory(this);
            }));
            members.AddRange(injectionMembers);

            this.Container.RegisterType<TInterface, TInstance>(Name, injectionMembers: members.ToArray());
        }

        public void RegisterType<TInterface, TInstance>(string Name, TInstance instance, params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.Add(new InjectionFactory((unity) => instance));
            members.AddRange(injectionMembers);

            this.Container.RegisterType<TInterface, TInstance>(Name, injectionMembers: members.ToArray());
        }

        public void RegisterSingleton<TInterface, TInstance>(string Name, TInterface instance, params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.Add(new InjectionFactory((unity) => instance));
            members.AddRange(injectionMembers);

            this.Container.RegisterSingleton<TInterface, TInstance>(Name, injectionMembers: members.ToArray());
        }

        public void RegisterSingleton<TInterface, TInstance>(string Name, Func<IResolver, TInterface> factory, params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.Add(new InjectionFactory((unity) =>
            {
                return factory(this);
            }));
            members.AddRange(injectionMembers);

            this.Container.RegisterSingleton<TInterface, TInstance>(Name, injectionMembers: members.ToArray());
        }

        public void RegisterSingleton<TInterface, TInstance>(string Name, object[] contructorParameters, params InjectionMember[] injectionMembers) where TInstance : TInterface
        {
            List<InjectionMember> members = this.GetDefaultInjectionMembers();
            members.Add(new InjectionConstructor(contructorParameters));
            members.AddRange(injectionMembers);

            this.Container.RegisterSingleton<TInterface, TInstance>(Name, injectionMembers: members.ToArray());
        }

        public void RegisterType<TInstance>(TInstance instance)
        {
            this.Container.RegisterType<TInstance>(new InjectionFactory((unity) =>
            {
                return instance;
            }));
        }

        public void RegisterType<TInstance>(string Name, TInstance instance)
        {
            this.Container.RegisterType<TInstance>(Name, new InjectionFactory((unity) =>
            {
                return instance;
            }));
        }

        public void RegisterSingleton<TInstance>(TInstance instance)
        {
            this.Container.RegisterSingleton<TInstance>(new InjectionFactory((unity) =>
            {
                return instance;
            }));
        }

        public void RegisterSingleton<TInstance>(string Name, TInstance instance)
        {
            this.Container.RegisterSingleton<TInstance>(new InjectionFactory((unity) =>
            {
                return instance;
            }));
        }
        public void RegisterSingleton<TInstance>()
        {
            this.Container.RegisterSingleton<TInstance>();
        }

        public void RegisterType<TInterface, TInstance>(string Name) where TInstance : TInterface
        {
            this.Container.RegisterType<TInterface, TInstance>(Name);
        }

        public void RegisterSingleton<TInterface, TInstance>(string Name) where TInstance : TInterface
        {
            this.Container.RegisterSingleton<TInterface, TInstance>(Name);
        }

        public void UseLogging(params FrameworkType[] types)
        {
            this.UseInterceptor(new FrameworkLogger(types, this.Resolve<ILogger>()));
        }
        public void UseInterceptor(IInterceptionBehavior interceptor)
        {
            this.Interceptors.Add(interceptor);
            this.Container.AddExtension(new InterceptorExtension(interceptor));
        }
        public void UseInterceptor<T>() where T: class, IInterceptionBehavior
        {
            this.UseInterceptor(Resolve<T>());
        }
       
        public T ApplyIntercepts<T>(T obj) where T:class
        {
            if (!typeof(T).IsInterface)
                return obj;

            T ReturnClass = Intercept.ThroughProxy(obj, new InterfaceInterceptor(),this.Interceptors);
            return ReturnClass;
        }
        public void RegisterSingleton(Type from, Type to, String name, Object[] contructorArguments)
        {
            if (Container.IsRegistered(from)) throw new Exception($"{from} is already registered");
            List<InjectionMember> members = new List<InjectionMember>();

            // TODO - Hector - Add dynamicly determined default injection types
            if(from.IsInterface)
                members = this.GetDefaultInjectionMembers();

            if (contructorArguments.Length > 0)
                members.Add(new InjectionConstructor(contructorArguments));

            try
            {
                this.Container.RegisterSingleton(from, to, name,  injectionMembers: members.ToArray());
            }
            catch (Exception e)
            {
                XConsole.WriteLine($"{e.Message}\n{e.StackTrace}");
            }

        }
        public void RegisterType(Type from, Type to, String name, Object[] contructorArguments)
        {
            if (Container.IsRegistered(from)) throw new Exception($"{from} is already registered");
            List<InjectionMember> members = new List<InjectionMember>();

            // TODO - Hector - Add dynamicly determined default injection types
            if (from.IsInterface)
                members = this.GetDefaultInjectionMembers();

            if (contructorArguments.Length > 0)
                members.Add(new InjectionConstructor(contructorArguments));

            try
            {
                this.Container.RegisterType(from, to, name,  injectionMembers: members.ToArray());
            }
            catch(Exception e)
            {
                XConsole.WriteLine($"{e.Message}\n{e.StackTrace}");
            }
            
            
        }
            
        #endregion
    }
}
