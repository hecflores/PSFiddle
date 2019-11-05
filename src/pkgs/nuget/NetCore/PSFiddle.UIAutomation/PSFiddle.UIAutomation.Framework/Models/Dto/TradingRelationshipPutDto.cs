using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    public class TradingRelationshipPutDto
    {
        /// <summary>
        /// Key of the relationship
        /// </summary>
        public long TradeRelationshipKey { get; set; }
        /// <summary>
        /// Remit from id of the relationship
        /// </summary>
        public long? RemitFromId { get; set; }
        /// <summary>
        /// Remit to id of the relationship
        /// </summary>
        public long? RemitToId { get; set; }
        /// <summary>
        /// Currency of this relationship
        /// </summary>
        public string CurrencyTo { get; set; }
        /// <summary>
        /// Currency of this relationship
        /// </summary>
        public string CurrencyFrom { get; set; }
        /// <summary>
        /// Org key of requesting user
        /// </summary>
        public long? OrgKey { get; set; }
        /// <summary>
        /// related org key for the notification to be sent to.
        /// </summary>
        public long? RelatedOrgKey { get; set; }
        /// <summary>
        /// User id of requesting user
        /// </summary>
        public long? UserId { get; set; }
    }
}
