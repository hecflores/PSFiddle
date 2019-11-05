using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Pages.Shared
{
    public interface IPageBase
    {
        void Setup(IResolver resolver);
        void ValidatePage();
        bool isSetup();
    }
}
