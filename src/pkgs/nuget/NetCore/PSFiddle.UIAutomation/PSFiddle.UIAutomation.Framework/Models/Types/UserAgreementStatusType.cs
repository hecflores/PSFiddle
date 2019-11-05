using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class UserAgreementStatusType
    {
        /// <summary>
        /// Key of the Agrrement Acceptance
        /// </summary>
        public long AgreementAcceptanceKey { get; set; }
        /// <summary>
        /// 1 = TOU, 2 = Data Privacy
        /// </summary>
        public int AgreementsKey { get; set; }
        /// <summary>
        /// TOU : Terms of Use PN: Privacy Notice
        /// </summary>
        public string TypeOfAgreement { get; set; }
        /// <summary>
        /// Alphs User ID
        /// </summary>
        public long AlphaUserKey { get; set; }
        /// <summary>
        /// Acceptance Status
        /// </summary>
        public bool IsAccepted { get; set; }
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
    }
}
