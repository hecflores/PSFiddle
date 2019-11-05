using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class NoMatchRecordAddress
    {
        public long key { get; set; }
        public String dateId { get; set; }
        public String city { get; set; }
        public String country { get; set; }
    }
    public class NoMatchRecordModel
    {
        public string companyKey { get; set; }
        public string dateId { get; set; }
        public string uniqueId { get; set; }
        public string registeredBusinessName { get; set; }
        public string description { get; set; }
        public string telephone { get; set; }
        public string website { get; set; }
        public string source { get; set; }
        public List<NoMatchRecordAddress> address { get; set; }

    }
}
