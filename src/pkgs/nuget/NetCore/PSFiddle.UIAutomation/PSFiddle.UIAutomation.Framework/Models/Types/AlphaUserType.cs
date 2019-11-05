
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MC.Track.TestSuite.Model.Types
{
    public class AlphaUserType
    {
        public long? AlphaUserKey { get; set; }
        public string UserID { get; set; }
        public string Jurisdiction_Code { get; set; }
        public string UserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Position { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public int? Current_Status { get; set; }
        public string Occupation { get; set; }
        public string Nationality { get; set; }
        public string UserEmail { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string FullAddress { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public long? ModifiedByKey { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long? CreatedByKey { get; set; }
        public long? OrganizationKey { get; set; }


    }
}