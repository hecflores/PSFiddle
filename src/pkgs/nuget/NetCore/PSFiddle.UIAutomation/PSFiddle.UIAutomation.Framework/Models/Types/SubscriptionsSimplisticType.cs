using MC.Track.TestSuite.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class SubscriptionsSimplisticType
    {
        public SubscriptionStatusReturnDto MakePaymentsStatus { get; set; }
        public bool IsSubscriberToMakePayments { get; set; }
        public bool IsSubscribeToMakePayments { get; set; }
        public SubscriptionStatusReturnDto ReceivePaymentsStatus { get; set; }
        public bool IsSubscriberToReceivePayments { get; set; }
        public bool IsSubscribeToReceivePayments { get; set; }
    }
}
