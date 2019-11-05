
namespace MC.Track.TestSuite.Model.Types
{
    public class NotificationRequest
    {
        /// <summary>
        /// User Key of user, who is sending notification.
        /// </summary>
        public long UserKey { get; set; }

        /// <summary>
        /// Organization Key to which notification should be sent.
        /// </summary>
        public long OrganizationKey { get; set; }


        /// <summary>
        /// Bank acocunt or card information that is added/changed/deleted in the action.
        /// </summary>
        public string ChangedInformation { get; set; }
    }
}
