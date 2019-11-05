using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.UI.Types
{
    public class OrganizationType 
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
       

        public long OrganizationKey { get; set; }
        public Nullable<long> ParentOrganizationKey { get; set; }
        public string AlphaID { get; set; }
        public string ParentAlphaID { get; set; }
        public string EIN { get; set; }
        public string TaxCode { get; set; }
        public string IDFromNetwork { get; set; }
        public string IDFromBuyer { get; set; }
        public string DunsNumber { get; set; }
        public string ComplianceVendorID { get; set; }
        public string CompanyNumber { get; set; }
        public string OrganizationName { get; set; }
        public string CurrentAlternativeLegalName { get; set; }
        public string Branch { get; set; }
        public string RegisteredBusinessName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string CompanyGeoLatLong { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string CompanyURL { get; set; }
        public Nullable<System.DateTime> DateOfCompanyRegistration { get; set; }
        public Nullable<System.DateTime> DateOfStartingOperation { get; set; }
        public string TypeOfOwnership { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhone { get; set; }
        public string ContactPersonName2 { get; set; }
        public string ContactPersonEmail2 { get; set; }
        public string ContactPersonPhone2 { get; set; }
        public string CompanyBusinessDescription { get; set; }
        public string IndustryDescription { get; set; }
        public string IndustryCode { get; set; }
        public string BusinessClassification { get; set; }
        public Nullable<int> NumberOfEmployees { get; set; }
        public Nullable<decimal> Revenue { get; set; }
        public string Certifications { get; set; }
        public string CertificateOfInsurance { get; set; }
        public string W9 { get; set; }
        public string F1099 { get; set; }
        public string BankingStatements { get; set; }
        public string FinancialStatements { get; set; }
        public string ISOCertificates { get; set; }
        public string SOC1Type2 { get; set; }
        public string IncorporationDocument { get; set; }
        public string JurisdictionCode { get; set; }
        public string NormalisedName { get; set; }
        public string CompanyType { get; set; }
        public Nullable<bool> Nonprofit { get; set; }
        public Nullable<long> CurrentStatus { get; set; }
        public Nullable<System.DateTime> DissolutionDate { get; set; }
        public string BusinessNumber { get; set; }
        public string CurrentAlternativeLegalNameLanguage { get; set; }
        public string HomeJurisdictiontext { get; set; }
        public string NativeCompanyNumber { get; set; }
        public string PreviousName { get; set; }
        public string AlternativeNames { get; set; }
        public Nullable<System.DateTime> RetrievedAt { get; set; }
        public string RegistryUrl { get; set; }
        public string RestrictedForMarketing { get; set; }
        public string RegisteredAddressInFull { get; set; }
        public string PrimaryUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<long> CreatedByKey { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<long> ModifiedByKey { get; set; }

       

      
    }
}
