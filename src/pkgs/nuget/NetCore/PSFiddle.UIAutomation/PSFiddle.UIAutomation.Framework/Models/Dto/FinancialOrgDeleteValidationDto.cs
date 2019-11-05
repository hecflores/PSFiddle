using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    public class FinancialOrgDeleteValidationDto
    {
        public string finInstOrgType { get; set; }
        public bool canBeDeleted { get; set; }
        public bool isDefault { get; set; }
        public List<Relationships> relationships { get; set;}
    }
}
