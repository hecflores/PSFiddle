namespace MC.Track.TestSuite.Model.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class AcquiringBankPostDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string BankID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MerchantID { get; set; }
        /// <summary>
        /// Bank's merchant ID
        /// </summary>
        public string BankMerchantID { get; set; }
        /// <summary>
        /// Currency for this acquirer account.
        /// </summary>
        public string Currency { get; set; }
    }
}
