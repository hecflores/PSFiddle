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
    public class MatchesGetResponse
    {
        /// <summary>
        /// Gets or sets the compliance unique identifier.
        /// </summary>
        /// <value>
        /// The compliance unique identifier.
        /// </value>
        public string ComplianceUniqueId { get; set; }
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; set; }
        /// <summary>
        /// Gets or sets the source i d1.
        /// </summary>
        /// <value>
        /// The source i d1.
        /// </value>
        public string SourceId1 { get; set; }
        /// <summary>
        /// Gets or sets the source i d2.
        /// </summary>
        /// <value>
        /// The source i d2.
        /// </value>
        public string SourceId2 { get; set; }
        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public decimal? MatchScore { get; set; }
        /// <summary>
        /// Gets or sets the match score rating.
        /// </summary>
        /// <value>
        /// The match score rating.
        /// </value>
        public string MatchScoreRating { get; set; }
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get; set; }
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }
        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>
        /// The zip.
        /// </value>
        public string Zip { get; set; }
        /// <summary>
        /// Gets or sets the company web URL.
        /// </summary>
        /// <value>
        /// The company web URL.
        /// </value>
        public string CompanyWebURL { get; set; }
        /// <summary>
        /// Gets or sets the credit rating.
        /// </summary>
        /// <value>
        /// The credit rating.
        /// </value>
        public string CreditRating { get; set; }
        /// <summary>
        /// Gets or sets the has been purchased.
        /// </summary>
        /// <value>
        /// The has been purchased.
        /// </value>
        public bool? IsPurchased { get; set; }
        /// <summary>
        /// Gets or sets the has value.
        /// </summary>
        /// <value>
        /// The has value.
        /// </value>
        public bool? HasComplianceRecord { get; set; }
        /// <summary>
        /// Gets or sets the has linked.
        /// </summary>
        /// <value>
        /// The has linked.
        /// </value>
        public bool? IsLinked { get; set; }
        /// <summary>
        /// Gets or sets the noof alerts.
        /// </summary>
        /// <value>
        /// The noof alerts.
        /// </value>
        public long? ComplianceAlertsCount { get; set; }
        /// <summary>
        /// Gets or sets the latest date.
        /// </summary>
        /// <value>
        /// The latest date.
        /// </value>
        public DateTime? LatestComplianceDate { get; set; }
    }
}
