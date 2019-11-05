using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Pages.Shared
{
    public interface IScopedPageUI : IPageBase
    {
        IRawPage Parent();
        void Setup(IResolver resolver, IProtectedWebElement scoped, IRawPage parentPage);
        IProtectedWebElement MyElement();
        IScoper DisabledScope();
    }
}
