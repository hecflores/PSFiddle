using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    /// <summary>
    /// Available Delete Card Actions
    /// </summary>
    public enum DeleteCardActions
    {   
        RequestDelete = 1,
        ApproveDelete = 2,
        RejectDelete = 3,
        CancelDelete = 4,
        AdminOverride = 5
    }

    public enum DeletePaymentType
    {
        CardToken = 1,
        MerchantId = 2,
    }
    public class DeleteCardTokenRequest
    {
        /// <summary>
        /// FinancialInstitutionOrganizationKey of the cart token record
        /// </summary>
        public long FinancialInstitutionOrganizationKey { get; set; }

        /// <summary>
        /// UserKey of user, who is making change.
        /// </summary>
        public long UserKey { get; set; }

        /// <summary>
        /// Action taken by user.
        /// </summary>
        public DeleteCardActions Action { get; set; }
        
        /// <summary>
        /// Payment type to be deleted
        /// </summary>
        public DeletePaymentType PaymentType { get; set; }

        /// <summary>
        /// Name of user, who initiated/approved/rejected/cancelled the request to delete
        /// </summary>
        public string ActorName { get; set; }
        /// <summary>
        /// Any extra query params needed to send emails for deep linking requests.
        /// </summary>
        public Dictionary<string, string> QueryParams { get; set; }
    }
}
