using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class UserAgreementType
    {
        /// <summary>
        /// TOU: Terms of Use, PN: (Privacy Notice) Data Privacy
        /// </summary>
        public string TypeofAgreement { get; set; }
        /// <summary>
        /// Agreement Key
        /// </summary>
        public long AgreementsKey { get; set; }
        /// <summary>
        /// Content user agreement (TOU/Data Privacy)
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        ///  version
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// To indicate if user has accepted the latest
        /// </summary>
        public long AcceptanceRequired { get; set; }
        /// <summary>
        /// Soecific Country Code
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// last modified date
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Modified By
        /// </summary>
        public string ModifiedBy { get; set; }
        /// <summary>
        /// User's Key modified agreement
        /// </summary>
        public long? ModifiedByKey { get; set; }
        /// <summary>
        /// Date TOU Was Created
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Created By
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// User's Key Created the agreement
        /// </summary>
        public long? CreatedByKey { get; set; }
    }
}
