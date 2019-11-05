using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Pages;
using MC.Track.TestSuite.Interfaces.Pages.Shared;



namespace MC.Track.TestSuite.Toolkit.Pages.Shared
{
    public class Pages : IPages
    {
        public ILoginPage LoginPage => CreatePage<ILoginPage>();
        


        private readonly IResolver resolver;
        public T CreatePage<T>() where T: class, IPageBase
        {
            var page = resolver.Resolve<T>();
            page.Setup(resolver);
            return page;
        }
        public Pages(IResolver resolver)
        {
            
            this.resolver = resolver;
        }


    }
}
