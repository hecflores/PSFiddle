using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyRecord
    {
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; set; }
        /// <summary>
        /// Gets or sets the source identifier i d1.
        /// </summary>
        /// <value>
        /// The source identifier .
        /// </value>
        public string SourceIdentifierId1 { get; set; }
        /// <summary>
        /// Gets or sets the source identifier i d2.
        /// </summary>
        /// <value>
        /// The source identifier i d2.
        /// </value>
        public string SourceIdentifierId2 { get; set; }

        /// <summary>
        /// Gets or sets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        public ulong OrganizationKey { get; set; }
    }
}
