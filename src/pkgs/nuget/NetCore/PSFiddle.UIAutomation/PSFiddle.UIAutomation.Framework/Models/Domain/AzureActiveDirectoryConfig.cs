using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Domain
{
    public class AzureActiveDirectoryConfig
    {
        public String Tenant { get; set; }
        public String ClientId { get; set; }
        public String AppKey { get; set; }
        public String ResourceId { get; set; }
        public bool UserSecurity { get; set; }
        public String AadInstance { get; set; }
        public String Authority { get => String.Format(CultureInfo.InvariantCulture, AadInstance, Tenant); set => throw new NotImplementedException(); }
        public String UserAuthString { get => String.Format(CultureInfo.InvariantCulture, AadInstance, "common/"); set => throw new NotImplementedException(); }
        public String UserClientId { get; set; }
        public String RedirectUri { get; set; }
    }
}
