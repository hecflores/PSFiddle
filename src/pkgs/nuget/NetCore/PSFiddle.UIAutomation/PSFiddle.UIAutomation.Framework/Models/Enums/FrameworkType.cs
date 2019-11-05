using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Enums
{

    public enum FrameworkType
    {
        TEST_SUITE          = (1 << 1),
        TEST_SUITE_SERVICES = (1 << 2),
        
        PAGES = (1 << 3),
        PAGE_FUNCTIONALALITY = (1 << 4),
        PAGE_BROWSER_FUNCTIONALITY = (1 << 5),
        PAGE_RAW_SELENIUM_FUNCTIONALITY = (1 << 6),

        DEPENDENCIES = (1 << 7),
        DEPENDENCIES_BUILDERS = (1 << 8),
        DEPENDENCIES_FUNCTIONAL_SERVICES = (1 << 9),
        DEPENDENCIES_REPOSITORY = (1 << 10),
        DEPENDENCIES_RAW_DATA_FUNCTIONS = (1 << 11)


    }
}
