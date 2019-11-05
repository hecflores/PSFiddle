namespace MC.Track.TestSuite.Model.Dto
{
    public class AcceptDeclineInvitationRequest
    {
        /// <summary>
        /// Organization key of the organization that was invited. 
        /// </summary>
        public long InvitedOrganization { get; set; }

        /// <summary>
        /// Organization key of the organization that invited other organization.
        /// </summary>
        public long InvitingOrganization { get; set; }

        /// <summary>
        /// User key that is accepting the invitation.
        /// </summary>
        public long UserKey { get; set; }

        /// <summary>
        /// Accept if true and Decline if false
        /// </summary>
        public bool AcceptDecline { get; set; }

        /// <summary>
        /// Specify values "Buyer" or "Supplier", if invited organization is buyer or supplier respectively.
        /// </summary>
        public string InvitedOrganizationType { get; set; }

        public const string InvitedOrganizationTypeBuyer = "Buyer";

        public const string InvitedOrganizationTypeSupplier = "Supplier";
    }
}
