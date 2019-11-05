using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Proxies
{
    public interface IFunctionProxy
    {

        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> BeforeCalled(Action<PreInvokedEventBasedProxyEventArgs> args);
        ISmartResourceDestroyerService<EventHandler<PostInvokedEventBasedProxyEventArgs>> AfterCalled(Action<PostInvokedEventBasedProxyEventArgs> args);
        ISmartResourceDestroyerService<EventHandler<PreInvokedEventBasedProxyEventArgs>> Disable();

    }
}
