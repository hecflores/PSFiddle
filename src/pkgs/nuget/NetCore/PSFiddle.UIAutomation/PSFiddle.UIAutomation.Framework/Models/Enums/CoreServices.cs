using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Enums
{
    public static class UiVariables
    {
        public const string InfoGroupValidSourceId = "InfoGroup ValidSourceId";
        public const string InfoGroupInValidSourceId = "InfoGroup InValidSourceId";

    }
    public class CoreServices
    {
        public static class Variables
        {
            public static class Common
            {
                public const string InputFile = "Variables InputFile";
                public const string FilePath = "Variables FilePath";
                public const string StorageBlobName = "Variables StorageBlobName";
                public const string Container = "Variables Container";
                public const string RelativePath = "Variables RelativePath";
                public const string StorageFilePath = "Variables StorageFilePath";
                public const string IngestedFileName = "Variables IngestedFileName";
                public const string IngestedCorrelationId = "Variables IngestedCorrelationId";
                public const string PIFScenario = "Variables PIFScenario";
                public const string BuyerUser = "BuyerUser";
                public const string SupplierUser = "SupplierUser";
            }
            public static class InvoiceXML
            {
                public const string SupplierAlphaID = "InvoiceXML SupplierAlphaID";
                public const string BuyerAlphaID = "InvoiceXML BuyerAlphaID";
                public const string SupplierName = "InvoiceXML SupplierName";
                public const string BuyerName = "InvoiceXML BuyerName";
                public const string PaymentDueDate = "InvoiceXML PaymentDueDate";
                public const string UUID = "InvoiceXML UUID";
                public const string InvoiceNumber = "InvoiceXML InvoiceNumber";
                public const string IssueDate = "InvoiceXML IssueDate";
                public const string InvoicePeriodStart = "InvoiceXML InvoicePeriodStart";
                public const string InvoicePeriodEnd = "InvoiceXML InvoicePeriodEnd";
                public const string PurchaseOrderNumber = "InvoiceXML PurchaseOrderNumber";
                public const string SupplierBankAccount = "InvoiceXML SupplierBankAccount";
                public const string InvoiceAmount = "InvoiceXML InvoiceAmount";
                public const string Currency = "InvoiceXML Currency";
                public const string PaymentChannelCode = "InvoiceXML PaymentChannelCode";
                public const string SupplierBankIdentification = "InvoiceXML SupplierBankIdentification";
            }
            public static class InvoiceDB
            {
                public const string SupplierAlphaID = "InvoiceDB SupplierAlphaID";
                public const string BuyerAlphaID = "InvoiceDB BuyerAlphaID";
                public const string SupplierName = "InvoiceDB SupplierName";
                public const string BuyerName = "InvoiceDB BuyerName";
                public const string PaymentDueDate = "InvoiceDB PaymentDueDate";
                public const string UUID = "InvoiceDB UUID";
                public const string InvoiceNumber = "InvoiceDB InvoiceNumber";
                public const string IssueDate = "InvoiceDB IssueDate";
                public const string InvoicePeriodStart = "InvoiceDB InvoicePeriodStart";
                public const string InvoicePeriodEnd = "InvoiceDB InvoicePeriodEnd";
                public const string PurchaseOrderNumber = "InvoiceDB PurchaseOrderNumber";
                public const string SupplierBankAccount = "InvoiceDB SupplierBankAccount";
                public const string InvoiceAmount = "InvoiceDB InvoiceAmount";
                public const string Currency = "InvoiceDB Currency";
                public const string PaymentChannelCode = "InvoiceDB PaymentChannelCode";
                public const string SupplierBankIdentification = "InvoiceDB SupplierBankIdentification";
            }

            public static class ContactsXML
            {
                public const string BuyerContactPersonEmail = "Contact Person Email";
                public const string BuyerContactPersonName = "Contact Person Name";
                public const string BuyerContactPersonPhone = "Contact Person Phone";
                public const string BuyerContactStreetAddress1 = "Contact Street Address1";
                public const string BuyerContactStreetAddress2 = "Contact Street Address2";
                public const string BuyerContactCity = "Contact City";
                public const string BuyerContactZip = "Contact Zip";
                public const string BuyerContactCountry = "Contact Country";
                public const string SupplierContactPersonEmail = "Contact Person Email";
                public const string SupplierContactPersonName = "Contact Person Name";
                public const string SupplierContactPersonPhone = "Contact Person Phone";
                public const string SupplierContactStreetAddress1 = "Contact Street Address1";
                public const string SupplierContactStreetAddress2 = "Contact Street Address2";
                public const string SupplierContactCity = "Contact City";
                public const string SupplierContactZip = "Contact Zip";
                public const string SupplierContactCountry = "Contact Country";
            }

            public static class ContactsDB
            {
                public const string ContactPersonEmail = "Contact Person Email";
                public const string ContactPersonName = "Contact Person Name";
                public const string ContactPersonPhone = "Contact Person Phone";
                public const string ContactStreetAddress1 = "Contact Street Address1";
                public const string ContactStreetAddress2 = "Contact Street Address2";
                public const string ContactCity = "Contact City";
                public const string ContactZip = "Contact Zip";
                public const string ContactCountry = "Contact Country";
            }
            public static class ResponseFile
            {
                public const string Exists = "ResponseFile Exist";
                public const string FileName = "ResponseFile FileName";
                public const string CorrelationID = "ResponseFile CorrelationID";
                public const string SourceFileID = "ResponseFile SourceFileID";
                public const string DebtorOrganizationID = "ResponseFile DebtorOrganizationID";
                public const string DebtorOrganiaztionName = "ResponseFile DebtorOrganiaztionName";
                public const string CreditorOrganizationID = "ResponseFile CreditorOrganizationID";
                public const string CreditorOrganizationName = "ResponseFile CreditorOrganizationName";
                public const string SourceFileStatusCode = "ResponseFile SourceFileStatusCode";
                public const string InstructionStatusCode = "ResponseFile InstructionStatusCode";
                public const string InstructionStatusDetails = "ResponseFile InstructionStatusDetails";
                public const string InstructionIdentification = "ResponseFile InstructionIdentification";
                public const string EndToEndIdentification = "ResponseFile EndToEndIdentification";
                public const string UUID = "ResponseFile UUID";
                public const string SourceFileIdentifier = "ResponseFile SourceFileIdentifier";
                public const string ResponseFileData = "ResponseFile Data";
            }
            public static class InvoiceResponseFile
            {
                public const string Exists = "InvoiceResponseFile Exist";
                public const string FileName = "InvoiceResponseFile FileName";
                public const string CorrelationID = "InvoiceResponseFile CorrelationID";
                public const string SourceFileID = "InvoiceResponseFile SourceFileID";
                public const string DebtorOrganizationID = "InvoiceResponseFile DebtorOrganizationID";
                public const string DebtorOrganiaztionName = "InvoiceResponseFile DebtorOrganiaztionName";
                public const string CreditorOrganizationID = "InvoiceResponseFile CreditorOrganizationID";
                public const string CreditorOrganizationName = "InvoiceResponseFile CreditorOrganizationName";
                public const string SourceFileStatusCode = "InvoiceResponseFile SourceFileStatusCode";
                public const string InstructionStatusCode = "InvoiceResponseFile InstructionStatusCode";
                public const string InstructionStatusDetails = "InvoiceResponseFile InstructionStatusDetails";
                public const string InstructionIdentification = "InvoiceResponseFile InstructionIdentification";
                public const string UUID = "InvoiceResponseFile UUID";
                public const string SourceFileIdentifier = "InvoiceResponseFile SourceFileIdentifier";
            }
            public static class Expectations
            {
                public const string CorrelationID = "Expectations CorrelationID";
                public const string SourceFileID = "Expectations SourceFileID";
                public const string BuyerAlphaID = "Expectations BuyerAlphaID";
                public const string BuyerName = "Expectations BuyerName";
                public const string SupplierAlphaID = "Expectations SupplierAlphaID";
                public const string SupplierName = "Expectations SupplierName";
                public const string StatusCode = "Expectations StatusCode";
                public const string TrasactionStatusCode = "Expectations TrasactionStatusCode";
                public const string TrasactionStatusDetails = "Expectations TrasactionStatusDetails";
                public const string InstructionIdentification = "Expectations InstructionIdentification";
                public const string EndToEndIdentification = "Expectations EndToEndIdentification";
                public const string UUID = "Expectations UUID";
            }

            

        }

        public static class Containers
        {
            public const string genesis = "genesis";
            public const string response = "response";
        }
        public static class PIFScenarios
        {
            public const string OnBehalf   = "OnBehalf";
            public const string Invoice    = "Invoice";
            public const string FileFormat = "FileFormat";
        }
        public static class TrasactionStatus
        {
            public static string TransactionReject = "RJCT";
            public static string TransactionAccept = "ACCP";
        }

        public static class ContactType
        {
            public static string Buyer = "Buyer";
            public static string Supplier = "Supplier";
        }
        public static class ErrorCode
        {
            public static string CH04 = "CH04";
            public static string CH20 = "CH20";
            public static string PO01 = "PO01";
            public static string DT01 = "DT01";
            public static string DT02 = "DT02";
            public static string DT03 = "DT03";
            public static string DT04 = "DT04";
            public static string DT05 = "DT05";
            public static string DU02 = "DU02";
            public static string DU04 = "DU04";
            public static string DU03 = "DU03";
            public static string UB01 = "UB01";
            public static string UB02 = "UB02";
            public static string UB03 = "UB03";
            public static string UB04 = "UB04";
            public static string UB05 = "UB05";
            public static string UB06 = "UB06";
            public static string UB07 = "UB07";
            public static string UB08 = "UB08";
            public static string RC06 = "RC06";
            public static string RC07 = "RC07";
            public static string AM01 = "AM01";
            public static string AM02 = "AM02";
            public static string AM11 = "AM11";
            public static string AM03 = "AM03";
            public static string AM12 = "AM12";
            public static string AM24 = "AM24";
            public static string RR02 = "RR02";
            public static string RR03 = "RR03";
        }

        public static class ErrorDescription
        {
            public static string CH04 = "PaymentDueDateInPast:Value in Payment Due Date is in the past.";
            public static string CH20 = "DecimalPointsNotCompatibleWithCurrency: Number of decimal points not compatible with the currency";
            public static string PO01 = "InvalidOrderReference: order Reference identifier is missing";
            public static string DT01 = "InvalidStartDate: Date is invalid or missing";
            public static string DT02 = "InvalidCreationDate: Date is invalid or missing";
            public static string DT03 = "InvalidEndDate: Date is invalid or missing";
            public static string DT04 = "FutureDateNotSupported: Future date not supported";
            public static string DT05 = "PaymentDueDateInvalid: Date is invalid or missing";
            public static string DU03 = "DuplicateTransaction: Transaction is not unique.";
            public static string UB01 = "VersionInvalid: UBL version is invalid or missing";
            public static string UB02 = "Source endpoint and Debtor/Buyer Identifier not matching";
            public static string UB03 = "InvalidLineAmount: Amount is invalid or missing";
            public static string UB04 = "ZeroAmount: Specified message amount is equal to zero";
            public static string UB05 = "DecimalPointsNotCompatibleWithCurrency: Number of decimal points not compatible with the currency";
            public static string UB06 = "InvoiceIdentifierMissing: InvoiceNumber is missing";
            public static string UB07 = "InvalidTransactionCurrency:Transaction currency is invalid (not a valid ISO Currency Code) or missing";
            public static string UB08 = "Itemdescription is missing";
            public static string RC06 = "InvalidDebtorIdentifier: Debtor/Buyer identifier is invalid or missing.";
            public static string RC07 = "InvalidCreditorIdentifier: Creditor/Supplier identifier is invalid or missing.";
            public static string AM01 = "ZeroAmount: Specified message amount is equal to zero";
            public static string AM02 = "NotAllowedAmount: Specific transaction/message amount is greater than allowed maximum (Future)";
            public static string AM11 = "InvalidTransactionCurrency:Transaction currency is invalid (not a valid ISO Currency Code) or missing.";
            public static string AM03 = "Specified transaction/message amount currency not in the existing agreement/relationship";
            public static string AM12 = "InvalidAmount: Amount is invalid or missing";
            public static string AM24 = "NoRelationship: Debtor/Buyer and Creditor/Supplier has no trade relationship.";
            public static string RR02 = "MissingDebtorName: Debtor/Buyer name is missing";
            public static string RR03 = "MissingCreditorName: Creditor/Supplier name is missing";
        }
    }
}
