using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Interception.InterceptionBehaviors;

namespace MC.Track.TestSuite.Interfaces.Proxies
{

    /// <summary>
    /// This class is very important. When it comes to Aspect Orianted Programming this interface is the solution to the problem of
    /// AOP implmentations. This class makes it easy to implment any type of cross cutting on any method. 
    /// </summary>
    public interface IEventBasedProxy: IInterceptionBehavior
    {
        EventHandler<PreInvokedEventBasedProxyEventArgs> PreInvokedSubscription { get; set; }
        EventHandler<PostInvokedEventBasedProxyEventArgs> PostInvokedSubscription { get; set; }

        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> BeforeCalled(Action<PreInvokedEventBasedProxyEventArgs> args);
        ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> AfterCalled(Action<PostInvokedEventBasedProxyEventArgs> args);


    }
}
