using MC.Track.TestSuite.Toolkit.Pages.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Toolkit.Pages
{
    public static class PageExtensions
    {
        public static void ClickChartItem(this RawPage page, String chartID, String category)
        {
            var position = page.RunDefaultJSModuleFunction<List<double>>("RuntimeModule", "GetPositionOfCategoryInChart", chartID, category);
            Assert.IsTrue(position.Count == 2, $"Expected position to have two array items but contained {position.Count}");
            page.ClickLocation($"Chart {chartID} , Category {category}", (int)position[0], (int)position[1]);
        }
    }
}
