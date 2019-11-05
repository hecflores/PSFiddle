using MC.Track.TestSuite.Interfaces.Proxies;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Proxies
{
    public class FunctionProxy: IFunctionProxy
    {
        private readonly IEventBasedProxy eventBasedProxy;

        public Type Type { get; }
        public MemberInfo Method { get; }
        public ISmartResourceDestroyerServiceFactory smartResourceDestroyerServiceFactory;
        public FunctionProxy(Type type, MemberInfo method, IEventBasedProxy eventBasedProxy, ISmartResourceDestroyerServiceFactory smartResourceDestroyerServiceFactory)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Method = method ?? throw new ArgumentNullException(nameof(method));
            this.eventBasedProxy = eventBasedProxy ?? throw new ArgumentNullException(nameof(eventBasedProxy));
            this.smartResourceDestroyerServiceFactory = smartResourceDestroyerServiceFactory;
        }
        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> BeforeCalled(Action<PreInvokedEventBasedProxyEventArgs> callback)
        {
            return eventBasedProxy.BeforeCalled((arg) =>
            {
                {
                    if (this.Type.IsInstanceOfType(arg.TargetObj))
                    {
                        if (this.Method.Name == arg.FunctionName)
                        {
                            callback(arg);
                        }
                    }
                }
            });
        }
        public ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> AfterCalled(Action<PostInvokedEventBasedProxyEventArgs> callback)
        {
            return eventBasedProxy.AfterCalled((arg) =>
            {
                {
                    if (this.Type.IsInstanceOfType(arg.TargetObj))
                    {
                        if (this.Method.Name == arg.FunctionName)
                        {
                            callback(arg);
                        }
                    }
                }
            });
        }

        public ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> Disable()
        {
            return BeforeCalled(arg =>
            {
                arg.Execute = false;
            });
        }
    }
}
