using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    public class InvitationPostDto
    {
        public long InviterOrganizationKey { get; set; }
        public bool IsBuyer { get; set; }
        public long UserKey { get; set; }

        public List<InviteDto> InvitedOrganizations { get; set; }
        
    }
    public class InviteDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long InvitedOrganizationkey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsOnboarded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }
        public long? StatusKey { get; set; }
        public string CorrelationId { get; set; }
        public string OrgName { get; set; }
    }
}
