using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFiddle.Common.Configurations
{
    public interface IConfiguration
    {
        string this[String key] { get; }

        String Get(String Key);
    }
}
