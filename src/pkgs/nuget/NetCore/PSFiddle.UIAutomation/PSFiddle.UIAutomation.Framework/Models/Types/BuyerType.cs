﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{ 
    public class BuyerType :BuyerSupplierType
    {
        public Decimal? PaymentScheduled { get; set; }
        public DateTime? PaymentDate { get; set; }
        public long PaymentInitiationInfoKey { get; set; }
        public long PaymentinitiationTxnsKey { get; set; }
    }
}
