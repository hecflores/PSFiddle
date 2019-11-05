namespace MC.Track.TestSuite.Model.Types
{
    public class LinkPostRequest
    {
        /// <summary>
        /// Gets or sets the compliance unique identifier.
        /// </summary>
        /// <value>
        /// The compliance unique identifier.
        /// </value>
        public string ComplianceUniqueId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }
    }
}