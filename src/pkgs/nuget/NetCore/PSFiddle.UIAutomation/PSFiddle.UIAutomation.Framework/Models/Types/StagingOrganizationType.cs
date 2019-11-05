using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class StagingOrganizationType     {
        /// <summary>
        /// Gets or sets the staging organization key.
        /// </summary>
        /// <value>
        /// The staging organization key.
        /// </value>
        public long? StagingOrganizationKey { get; set; }
        /// <summary>
        /// Gets or sets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        public string OrganizationKey { get; set; }
        /// <summary>
        /// Gets or sets the parent organization key.
        /// </summary>
        /// <value>
        /// The parent organization key.
        /// </value>
        public long? ParentOrganizationKey { get; set; }
        /// <summary>
        /// Gets or sets the parent type organization key.
        /// </summary>
        /// <value>
        /// The type of organization key.
        /// </value>
        public long? OrganizationTypeKey { get; set; }
        /// <summary>
        /// Gets or sets the alpha identifier.
        /// </summary>
        /// <value>
        /// The alpha identifier.
        /// </value>
        public string AlphaID { get; set; }
        /// <summary>
        /// Gets or sets the parent alpha identifier.
        /// </summary>
        /// <value>
        /// The parent alpha identifier.
        /// </value>
        public string ParentAlphaID { get; set; }
        /// <summary>
        /// Gets or sets the ein.
        /// </summary>
        /// <value>
        /// The ein.
        /// </value>
        public string EIN { get; set; }
        /// <summary>
        /// Gets or sets the tax code.
        /// </summary>
        /// <value>
        /// The tax code.
        /// </value>
        public string TaxCode { get; set; }
        /// <summary>
        /// Gets or sets the identifier from network.
        /// </summary>
        /// <value>
        /// The identifier from network.
        /// </value>
       
        public string IDFromNetwork { get; set; }
        /// <summary>
        /// Gets or sets the identifier from buyer.
        /// </summary>
        /// <value>
        /// The identifier from buyer.
        /// </value>
        public string IDFromBuyer { get; set; }
        /// <summary>
        /// Gets or sets the duns number.
        /// </summary>
        /// <value>
        /// The duns number.
        /// </value>
        public string DunsNumber { get; set; }
        /// <summary>
        /// Gets or sets the compliance vendor identifier.
        /// </summary>
        /// <value>
        /// The compliance vendor identifier.
        /// </value>
        public string ComplianceVendorID { get; set; }
        /// <summary>
        /// Gets or sets the company number.
        /// </summary>
        /// <value>
        /// The company number.
        /// </value>
        public string CompanyNumber { get; set; }
        /// <summary>
        /// Gets or sets the name of the organization.
        /// </summary>
        /// <value>
        /// The name of the organization.
        /// </value>
        public string OrganizationName { get; set; }
        /// <summary>
        /// Gets or sets the name of the current alternative legal.
        /// </summary>
        /// <value>
        /// The name of the current alternative legal.
        /// </value>
        public string CurrentAlternativeLegalName { get; set; }
        /// <summary>
        /// Gets or sets the branch.
        /// </summary>
        /// <value>
        /// The branch.
        /// </value>
        public string Branch { get; set; }
        /// <summary>
        /// Gets or sets the name of the registered business.
        /// </summary>
        /// <value>
        /// The name of the registered business.
        /// </value>
        public string RegisteredBusinessName { get; set; }
        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>
        /// The address1.
        /// </value>
        public string Address1 { get; set; }
        /// <summary>
        /// Gets or sets the address2.
        /// </summary>
        /// <value>
        /// The address2.
        /// </value>
        public string Address2 { get; set; }
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
        /// Gets or sets the zip.
        /// </summary>
        /// <value>
        /// The zip.
        /// </value>
        public string Zip { get; set; }
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }
        /// <summary>
        /// Gets or sets the company geo lat long.
        /// </summary>
        /// <value>
        /// The company geo lat long.
        /// </value>
        public string CompanyGeoLatLong { get; set; }
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the fax number.
        /// </summary>
        /// <value>
        /// The fax number.
        /// </value>
        public string FaxNumber { get; set; }
        /// <summary>
        /// Gets or sets the company URL.
        /// </summary>
        /// <value>
        /// The company URL.
        /// </value>
        public string CompanyURL { get; set; }
        /// <summary>
        /// Gets or sets the date of company registration.
        /// </summary>
        /// <value>
        /// The date of company registration.
        /// </value>
        public DateTime? DateOfCompanyRegistration { get; set; }
        /// <summary>
        /// Gets or sets the date of starting operation.
        /// </summary>
        /// <value>
        /// The date of starting operation.
        /// </value>
        public DateTime? DateOfStartingOperation { get; set; }
        /// <summary>
        /// Gets or sets the type of ownership.
        /// </summary>
        /// <value>
        /// The type of ownership.
        /// </value>
        public string TypeOfOwnership { get; set; }
        /// <summary>
        /// Gets or sets the name of the contact person.
        /// </summary>
        /// <value>
        /// The name of the contact person.
        /// </value>
        public string ContactPersonName { get; set; }
        /// <summary>
        /// Gets or sets the contact person email.
        /// </summary>
        /// <value>
        /// The contact person email.
        /// </value>
        public string ContactPersonEmail { get; set; }
        /// <summary>
        /// Gets or sets the contact person phone.
        /// </summary>
        /// <value>
        /// The contact person phone.
        /// </value>
        public string ContactPersonPhone { get; set; }
        /// <summary>
        /// Gets or sets the contact person name2.
        /// </summary>
        /// <value>
        /// The contact person name2.
        /// </value>
        public string ContactPersonName2 { get; set; }
        /// <summary>
        /// Gets or sets the contact person email2.
        /// </summary>
        /// <value>
        /// The contact person email2.
        /// </value>
        public string ContactPersonEmail2 { get; set; }
        /// <summary>
        /// Gets or sets the contact person phone2.
        /// </summary>
        /// <value>
        /// The contact person phone2.
        /// </value>
        public string ContactPersonPhone2 { get; set; }
        /// <summary>
        /// Gets or sets the company business description.
        /// </summary>
        /// <value>
        /// The company business description.
        /// </value>
        public string CompanyBusinessDescription { get; set; }
        /// <summary>
        /// Gets or sets the industry description.
        /// </summary>
        /// <value>
        /// The industry description.
        /// </value>
        public string IndustryDescription { get; set; }
        /// <summary>
        /// Gets or sets the industry code.
        /// </summary>
        /// <value>
        /// The industry code.
        /// </value>
        public string IndustryCode { get; set; }
        /// <summary>
        /// Gets or sets the business classification.
        /// </summary>
        /// <value>
        /// The business classification.
        /// </value>
        public string BusinessClassification { get; set; }
        /// <summary>
        /// Gets or sets the number of employees.
        /// </summary>
        /// <value>
        /// The number of employees.
        /// </value>
        public int? NumberOfEmployees { get; set; }
        /// <summary>
        /// Gets or sets the revenue.
        /// </summary>
        /// <value>
        /// The revenue.
        /// </value>
        public decimal? Revenue { get; set; }
        /// <summary>
        /// Gets or sets the certifications.
        /// </summary>
        /// <value>
        /// The certifications.
        /// </value>
        public string Certifications { get; set; }
        /// <summary>
        /// Gets or sets the certificate of insurance.
        /// </summary>
        /// <value>
        /// The certificate of insurance.
        /// </value>
        public string CertificateOfInsurance { get; set; }
        /// <summary>
        /// Gets or sets the w9.
        /// </summary>
        /// <value>
        /// The w9.
        /// </value>
        public string W9 { get; set; }
        /// <summary>
        /// Gets or sets the F1099.
        /// </summary>
        /// <value>
        /// The F1099.
        /// </value>
        public string F1099 { get; set; }
        /// <summary>
        /// Gets or sets the banking statements.
        /// </summary>
        /// <value>
        /// The banking statements.
        /// </value>
        public string BankingStatements { get; set; }
        /// <summary>
        /// Gets or sets the financial statements.
        /// </summary>
        /// <value>
        /// The financial statements.
        /// </value>
        public string FinancialStatements { get; set; }
        /// <summary>
        /// Gets or sets the iso certificates.
        /// </summary>
        /// <value>
        /// The iso certificates.
        /// </value>
        public string ISOCertificates { get; set; }
        /// <summary>
        /// Gets or sets the so c1 type2.
        /// </summary>
        /// <value>
        /// The so c1 type2.
        /// </value>
        public string SOC1Type2 { get; set; }
        /// <summary>
        /// Gets or sets the incorporation document.
        /// </summary>
        /// <value>
        /// The incorporation document.
        /// </value>
        public string IncorporationDocument { get; set; }
        /// <summary>
        /// Gets or sets the jurisdiction code.
        /// </summary>
        /// <value>
        /// The jurisdiction code.
        /// </value>
        public string JurisdictionCode { get; set; }
        /// <summary>
        /// Gets or sets the name of the normalised.
        /// </summary>
        /// <value>
        /// The name of the normalised.
        /// </value>
        public string NormalisedName { get; set; }
        /// <summary>
        /// Gets or sets the type of the company.
        /// </summary>
        /// <value>
        /// The type of the company.
        /// </value>
        public string CompanyType { get; set; }
        /// <summary>
        /// Gets or sets the nonprofit.
        /// </summary>
        /// <value>
        /// The nonprofit.
        /// </value>
        public bool? Nonprofit { get; set; }
        /// <summary>
        /// Gets or sets the current status.
        /// </summary>
        /// <value>
        /// The current status.
        /// </value>
        public long? Current_Status { get; set; }
        /// <summary>
        /// Gets or sets the dissolution date.
        /// </summary>
        /// <value>
        /// The dissolution date.
        /// </value>
        public DateTime? DissolutionDate { get; set; }
        /// <summary>
        /// Gets or sets the business number.
        /// </summary>
        /// <value>
        /// The business number.
        /// </value>
        public string BusinessNumber { get; set; }
        /// <summary>
        /// Gets or sets the current alternative legal name language.
        /// </summary>
        /// <value>
        /// The current alternative legal name language.
        /// </value>
        public string CurrentAlternativeLegalNameLanguage { get; set; }
        /// <summary>
        /// Gets or sets the home jurisdictiontext.
        /// </summary>
        /// <value>
        /// The home jurisdictiontext.
        /// </value>
        public string HomeJurisdictiontext { get; set; }
        /// <summary>
        /// Gets or sets the native company number.
        /// </summary>
        /// <value>
        /// The native company number.
        /// </value>
        public string NativeCompanyNumber { get; set; }
        /// <summary>
        /// Gets or sets the name of the previous.
        /// </summary>
        /// <value>
        /// The name of the previous.
        /// </value>
        public string PreviousName { get; set; }
        /// <summary>
        /// Gets or sets the alternative names.
        /// </summary>
        /// <value>
        /// The alternative names.
        /// </value>
        public string AlternativeNames { get; set; }
        /// <summary>
        /// Gets or sets the retrieved at.
        /// </summary>
        /// <value>
        /// The retrieved at.
        /// </value>
        public DateTime? RetrievedAt { get; set; }
        /// <summary>
        /// Gets or sets the registry URL.
        /// </summary>
        /// <value>
        /// The registry URL.
        /// </value>
        public string RegistryUrl { get; set; }
        /// <summary>
        /// Gets or sets the restricted for marketing.
        /// </summary>
        /// <value>
        /// The restricted for marketing.
        /// </value>
        public string RestrictedForMarketing { get; set; }
        /// <summary>
        /// Gets or sets the registered address in full.
        /// </summary>
        /// <value>
        /// The registered address in full.
        /// </value>
        public string RegisteredAddressInFull { get; set; }
        /// <summary>
        /// Gets or sets the primary user identifier.
        /// </summary>
        /// <value>
        /// The primary user identifier.
        /// </value>
        public string PrimaryUserID { get; set; }
        /// <summary>
        /// Gets or sets the current org identifier.
        /// </summary>
        /// <value>
        /// The current org identifier.
        /// </value>
        public string CurrentOrgID { get; set; }
        /// <summary>
        /// Gets or sets the type of the current org.
        /// </summary>
        /// <value>
        /// The type of the current org.
        /// </value>
        public string CurrentOrgType { get; set; }
        /// <summary>
        /// Gets or sets the related org identifier.
        /// </summary>
        /// <value>
        /// The related org identifier.
        /// </value>
        public string RelatedOrgID { get; set; }
        /// <summary>
        /// Gets or sets the type of the related org.
        /// </summary>
        /// <value>
        /// The type of the related org.
        /// </value>
        public string RelatedOrgType { get; set; }
        /// <summary>
        /// Gets or sets the phone type.
        /// </summary>
        /// <value>
        /// The phone type.
        /// </value>
        public string PhoneType1 { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public string PhoneType2 { get; set; }
        /// <summary>
        /// Gets or sets the phone type3.
        /// </summary>
        /// <value>
        /// The phone type3.
        /// </value>
        public string PhoneType3 { get; set; }
        /// <summary>
        /// Gets or sets the contact person phone3.
        /// </summary>
        /// <value>
        /// The contact person phone 3.
        /// </value>
        public string ContactPersonPhone3 { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [on behalf payment].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [on behalf payment]; otherwise, <c>false</c>.
        /// </value>
        public bool? OnBehalfPayment { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the created by key.
        /// </summary>
        /// <value>
        /// The created by key.
        /// </value>
        public long? CreatedByKey { get; set; }
        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        public string ModifiedBy { get; set; }
        /// <summary>
        /// Gets or sets the modified by key.
        /// </summary>
        /// <value>
        /// The modified by key.
        /// </value>
        public long? ModifiedByKey { get; set; }

        public String DoingBusinessAs { get; set; }
        public String DataSource { get; set; }
        public String NAICSCode { get; set; }
        public String VAT { get; set; }
        public String SourceIdentifier { get; set; }
        public String SourceIdentifierId1 { get; set; }
        public String SourceIdentifierId2 { get; set; }
        public String SourceIdentifierKey { get; set; }
    }
}
