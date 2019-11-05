using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    public abstract class TradingRelationshipDto
    {
        /// <summary>
        /// First organization
        /// </summary>
        public long? PrimaryOrganizationKey { get; set; }
        //First org's role
        public long? PrimaryOrganizationTypeKey { get; set; }
        /// <summary>
        /// Second org
        /// </summary>
        public long? SecondaryOrganizationKey { get; set; }
        /// <summary>
        /// Second org's role
        /// </summary>
        public long? SecondaryOrganizationTypeKey { get; set; }
        /// <summary>
        /// Remit to ID of the relationship
        /// </summary>
        public long? RemitToId { get; set; }
        /// <summary>
        /// Remit from ID of the relationship
        /// </summary>
        public long? RemitFromId { get; set; }
        /// <summary>
        /// Currency from
        /// </summary>
        public string CurrencyFrom { get; set; }
        /// <summary>
        /// Currency to
        /// </summary>
        public string CurrencyTo { get; set; }
    }
}
