using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
	/// <summary>
    /// 
    /// </summary>
    public class StagingRelationshipDto
    {
		/// <summary>
        /// 
        /// </summary>
        public long? StagingOrganizationKey { get; set; }
		/// <summary>
        /// 
        /// </summary>
        public long? OrganizationKey { get; set; }
		/// <summary>
        /// 
        /// </summary>
        public string ParameterValue { get; set; }
		/// <summary>
        /// 
        /// </summary>
        public long? StagingOrganizationStatusValue { get; set; }
    }
}