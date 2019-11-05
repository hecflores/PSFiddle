using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime InvoicePeriodStart { get; set; }
        public DateTime InvoicePeriodEnd { get; set; }
        public string PurchaseOrderNumber { get; set; }

        public string SupplierAlphaID { get; set; }
        public string SupplierName { get; set; }
        public string BuyerAlphaID { get; set; }
        public string BuyerName { get; set; }

        public string SupplierBankAccount { get; set; }

        public DateTime PaymentDueDate { get; set; }

        public double InvoiceAmount { get; set; }

        public string PaymentChannelCode { get; set; }

        public string SupplierBankIdentification { get; set; }

        public string currency { get; set; }

        //Sprint 15 changes
        public string UUID { get; set; }


    }
    public enum InvoiceAttributes
    {
        SupplierAlphaID,
        BuyerAlphaID,
        SupplierName,
        BuyerName,
        PaymentDueDate,
        UUID,
        InvoiceNumber
    }
}
