using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Pages.Shared;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class DefinedBrowserAttribute : ResourceDependencyAttribute
    {
        private readonly string name;
        private readonly bool initialBrowser;
        private readonly string loginUser;
        private readonly bool start;

        public DefinedBrowserAttribute(String Name,
                                       bool InitialBrowser = false, 
                                       String LoginUser = null,
                                       bool Start = false)
        {
            name = Name;
            initialBrowser = InitialBrowser;
            loginUser = LoginUser;
            start = Start;
        }
        public override void Setup(IResolver resolver)
        {
            var browser = resolver.Resolve<IDependencies>().Browser().Factory().IncognitoChromeBrowser((builder) =>
            {
                if (this.initialBrowser || this.loginUser!=null)
                    builder.Focus();

                if (this.start || this.loginUser != null)
                    builder.StartBrowser();

                if (this.loginUser != null)
                    builder.AddBuildStep("Login User", (browserItem) =>
                    {
                        var user = resolver.Resolve<IStateManagment>().Get<UserType>(this.loginUser);
                        // resolver.Resolve<IPages>().LoginPage.LoginAs(user);
                        return browserItem;
                    });
            }).BuildSingle();

            resolver.Resolve<IStateManagment>().Set(name, browser);
        }
    }
}
