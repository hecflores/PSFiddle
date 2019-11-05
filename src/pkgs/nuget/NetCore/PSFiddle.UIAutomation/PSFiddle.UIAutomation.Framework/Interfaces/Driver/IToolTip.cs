using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Driver
{
    public interface IToolTip
    {
        bool HasToolTip();
        string GetTooltipText();
    }
}
