using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    public class BuyerSupplierDto
    {
        public long? OrganizationKey { get; set; }
        public long? StagingOrganizationKey { get; set; }
        public string AlphaId { get; set; }
        public string ParentAlphaID { get; set; }
        public string EIN { get; set; }
        public string TaxCode { get; set; }
        public string IDFromNetwork { get; set; }
        public string IDFromBuyer { get; set; }
        public string DunsNumber { get; set; }
        public string ComplianceVendorId { get; set; }
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
        public string CompanyUrl { get; set; }
        public string DateOfCompanyRegistration { get; set; }
        public string DateOfStartingOperation { get; set; }
        public string TypeOfOwnership { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhone { get; set; }
        public string ComplianceVendorID { get; set; }
        public long PrimaryUserId { get; set; }
        public bool IsFavourite { get; set; }
        public bool IsFollowed { get; set; }
        public int NoOfComplianceAlerts { get; set; }
        public int NoOfIdentityAlerts { get; set; }
        public string TradingStatus { get; set; }
    }

}
