using MC.Track.Shared;
using PSFiddle.UIAutomation.Framework.Shared;
using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Pages;
using MC.Track.TestSuite.Interfaces.Pages.Shared;
using MC.Track.TestSuite.Interfaces.Services;
using OpenQA.Selenium;

namespace MC.Track.TestSuite.Toolkit.Pages.Shared
{
    public class PaginationControls<T> : ScopedPageUI, IPaginationControls<T> where T : class, IPageBase
    {
        public PaginationControls(): base()
        {
        }

        public T NextPage<T2>() where T2: class, T
        {
            ClickElement(LocatorNames.Pagination_Next);

            //  TODO - Hector, how do I do this? It just returns an instance of the current page type with different data
            //return PageNewer.Create<ILayoutPage, IWebElement>(this.resolver);
            return TransitionPage<T2>();
        }

        public T PreviousPage<T2>() where T2 : class, T
        {
            ClickElement(LocatorNames.Pagination_Previous);

            //  TODO - Hector, how do I do this? It just returns an instance of the current page type with different data
            //return PageNewer.Create<ILayoutPage, IWebElement>(this.resolver);
            return TransitionPage<T2>();
        }

        public T NthPage<T2>(int pageNumber) where T2 : class, T
        {
            ClickElement(LocatorNames.Pagination_Next);

            //  TODO - Hector, how do I do this? It just returns an instance of the current page type with different data
            //return PageNewer.Create<ILayoutPage, IWebElement>(this.resolver);
            return TransitionPage<T2>();
        }

        public override void ValidatePage()
        {
        }
    }
}
