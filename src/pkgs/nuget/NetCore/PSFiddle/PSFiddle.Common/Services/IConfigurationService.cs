using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.Common.Services
{
    public interface IConfigurationService
    {
        String Get(String config);
    }
}
