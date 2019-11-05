using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Pages;
using MC.Track.TestSuite.Interfaces.Pages.Shared;


using MC.Track.TestSuite.Interfaces.Driver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MC.Track.TestSuite.Toolkit.Pages.Shared
{
    public abstract class ScopedPageUI : RawPage, IScopedPageUI
    {
        private IWebElementWrapper scoped;
        private IGenericScopingFactory genericScopingFactory;
        private bool scopingEnabled = true;
        private IRawPage parentPage;

        
        public void Setup(IResolver resolver, IProtectedWebElement scoped, IRawPage parentPage)
        {
            this.scoped = scoped.Unprotected();
            this.genericScopingFactory = resolver.Resolve<IGenericScopingFactory>();
            this.parentPage = parentPage;

            base.Setup(resolver);
        }
        
        public IRawPage Parent()
        {
            return parentPage;
        }
        public override T TransitionPage<T>() 
        {
            using (this.DisabledScope())
            {
                return base.TransitionPage<T>();
            }
            
        }
        protected IProtectedWebElement MyElement()
        {
            return scoped;
        }
        private IScoper fakeScoper()
        {
            return this.genericScopingFactory.Create(() => { });
        }
        public override IScoper Scope(IProtectedWebElement webElement)
        {
            if(scopingEnabled) return base.Scope(webElement);
            return fakeScoper();
        }
        public virtual void ClickChartItem(String ChartItem)
        {
            WaitFor(() =>
            {
                Assert.IsNotNull(this.MyElement().Unprotected().GetAttribute("id"), "Expected Chart to have the 'id' attribute for identification but didnt");
                this.ClickChartItem($"#{this.MyElement().Unprotected().GetAttribute("id")}", ChartItem);
            });
        }
        public virtual void VerifyChartIsLoaded()
        {
            WaitFor(() => {
                var timeout = 30;
                var ChartID = $"#{this.MyElement().Unprotected().GetAttribute("id")}";
                List<String> categories = RunDefaultJSModuleFunction<List<String>>("RuntimeModule", "GetChartCategories", ChartID);
                Assert.IsTrue(categories.Count > 0, $"Chart not loaded: {MyElement().Name}");
            });
        }
        public virtual void VerifyChartCategoryExists(String Category)
        {
            WaitFor(() => {
                var timeout = 30;
                var ChartID = $"#{this.MyElement().Unprotected().GetAttribute("id")}";
                List<String> categories = RunDefaultJSModuleFunction<List<String>>("RuntimeModule", "GetChartCategories", ChartID);
                Assert.IsTrue(categories.Count > 0, $"Chart not loaded: {MyElement().Name}");
                Assert.IsTrue(categories.Contains(Category), $"Category {Category} was not found in chart but was expected: '{String.Join(",", categories)}'");
            });
        }
        public virtual void VerifyChartCategoryDoesNotExists(String Category)
        {
            WaitFor(() => {
                var timeout = 30;
                var ChartID = $"#{this.MyElement().Unprotected().GetAttribute("id")}";
                List<String> categories = RunDefaultJSModuleFunction<List<String>>("RuntimeModule", "GetChartCategories", ChartID);
                Assert.IsTrue(categories.Count > 0, $"Chart not loaded: {MyElement().Name}");
                Assert.IsFalse(categories.Contains(Category), $"Category {Category} was not found in chart but was expected: '{String.Join(",", categories)}'");
            });
        }
        public IScoper DisabledScope()
        {
            this.scopingEnabled = false; // Stop Scoping...
            return this.genericScopingFactory.Create(() => {
                this.scopingEnabled = true; // Allow Scoping...
            });
        }
        public override void ExpectSpinner()
        {
            using (this.DisabledScope())
            {
                base.ExpectSpinner();
            }
        }
        public override void ClickLocation(string Name, int x, int y)
        {
            using (base.Scope(this.scoped))
            {
                base.ClickLocation(Name, x, y);
            }
        }
        public override void ExpectNoSpinner()
        {
            using (this.DisabledScope())
            {
                base.ExpectNoSpinner();
            }
        }
        public override void VerifyElementIsHidden(String Name)
        {
            using (this.Scope(this.scoped))
            {
                base.VerifyElementIsHidden(Name);
            }
        }
        public override void VerifyElementIsVisable(String Name)
        {
            using (this.Scope(this.scoped))
            {
                base.VerifyElementIsVisable(Name);
            }
        }
        public override void VerifyElementNotExists(string Name)
        {
            using (this.Scope(this.scoped))
            {
                base.VerifyElementNotExists(Name);
            }
        }
        public override bool IsSelected(string Name)
        {
            using (this.Scope(this.scoped))
            {
               return base.IsSelected(Name);
            }
        }
        public override void VerifyIsSelected(String Name)
        {
            using (this.Scope(this.scoped))
            {
                base.VerifyIsSelected(Name);
            }
        }
        public override void VerifyIsNotSelected(String Name)
        {
            using (this.Scope(this.scoped))
            {
                base.VerifyIsNotSelected(Name);
            }
        }
        public override void ClickElement(string ElementName)
        {
            using (this.Scope(this.scoped))
            {
                base.ClickElement(ElementName);
            }
        }
        public override String Text(string ElementName)
        {
            using (this.Scope(this.scoped))
            {
                return base.Text(ElementName);
            }
        }
        public override List<IProtectedWebElement> GetElements(string Name, int minimumFound = 0, int maximumFound = Int32.MaxValue)
        {
            using (this.Scope(this.scoped))
            {
                return base.GetElements(Name, minimumFound, maximumFound);
            }
        }
        public override void TypeIn(string Text, string ElementName)
        {
            using (this.Scope(this.scoped))
            {
                base.TypeIn(Text, ElementName);
            }
        }
        public override void VerifyElement(string Name)
        {
            using (this.Scope(this.scoped))
            {
                base.VerifyElement(Name);
            }
        }

        IProtectedWebElement IScopedPageUI.MyElement()
        {
            return this.MyElement();
        }
    }
}
