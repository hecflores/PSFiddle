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
    public class OrganizationComplianceCompanyLinkingType : BaseDBType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationComplianceCompanyLinkingType"/> class.
        /// </summary>
        public OrganizationComplianceCompanyLinkingType()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationComplianceCompanyLinkingType"/> class.
        /// </summary>
        /// <param name="companyRecord">The company record.</param>
        /// <param name="complianceRecord">The compliance record.</param>
        public OrganizationComplianceCompanyLinkingType(CompanyRecord companyRecord, LinkPostRequest complianceRecord)
        {
            Source = companyRecord.Source;
            SourceID1 = companyRecord.SourceIdentifierId1;
            SourceID2 = companyRecord.SourceIdentifierId2;
            OrganizationKey = (long)companyRecord.OrganizationKey;
            ComplianceUniqueID = complianceRecord.ComplianceUniqueId;
            IsActive = complianceRecord.IsActive;

        }
        /// <summary>
        /// Gets or sets the organization compliance company linking key.
        /// </summary>
        /// <value>
        /// The organization compliance company linking key.
        /// </value>
        public long? OrganizationComplianceCompanyLinkingKey { get; set; }
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; set; }
        /// <summary>
        /// Gets or sets the source identifier i d1.
        /// </summary>
        /// <value>
        /// The source identifier .
        /// </value>
        public string SourceID1 { get; set; }
        /// <summary>
        /// Gets or sets the source identifier i d2.
        /// </summary>
        /// <value>
        /// The source identifier i d2.
        /// </value>
        public string SourceID2 { get; set; }
        /// <summary>
        /// Gets or sets the compliance unique identifier.
        /// </summary>
        /// <value>
        /// The compliance unique identifier.
        /// </value>
        public string ComplianceUniqueID { get; set; }
        /// <summary>
        /// Gets or sets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        public long OrganizationKey { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

    }
}
