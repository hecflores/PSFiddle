using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class FileUploadInput
    {
        public long RawOrganizationKey { get; set; }
        public string CorrelationID { get; set; }
        public string ComplianceVendorID { get; set; }
        public string RegisteredBusinessName { get; set; }
        public string StreetAddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public decimal SpendPurchaseAmount { get; set; }
        public string PriorityLevel { get; set; }
        public string PhoneNumber { get; set; }
        public string ParentCompanyName { get; set; }
        public string ParentCompanyAddress { get; set; }
        public string ParentCompanyAddress1 { get; set; }
        public string ParentCompanyAddress2 { get; set; }
        public string ParentCompanyAddress3 { get; set; }
        public string ParentCompanyCity { get; set; }
        public string ParentCompanyState { get; set; }
        public string ParentCompanyCountry { get; set; }
        public string ParentCompanyZip { get; set; }
        public string CompanyURL { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string EIN { get; set; }
        public string TIN { get; set; }
        public string VAT { get; set; }
        public string RegistrationNumber { get; set; }
        public string StreetAddress2 { get; set; }
        public string City2 { get; set; }
        public string State2 { get; set; }
        public string Country2 { get; set; }
        public string Zip2 { get; set; }
        public long CreatedByKey { get; set; }
        public long ModifiedByKey { get; set; }
    }
}
