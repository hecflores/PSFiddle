
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace MC.Track.Shared
{
    public class UIElements
    {
        private static String AutoDefine(String Name, [CallerMemberName] String varName = "")
        {
            return $"UIElement |{Name}|==|//*[@ui-element='{varName}']|";
        }

        #region UIElements

        #region Search Directory Organization
        public static String Advanced_Search_Toggle = AutoDefine("Advanced Search Page - Search Toggle");
        public static String Search_Directory_Organization_Row = AutoDefine("Search Directory - Row");
        public static String Search_Directory_Organization_ColumnCheckbox = AutoDefine("Search Directory - Row Column (Check Box)");
        public static String Search_Directory_Organization_ColumnCompanyName = AutoDefine("Search Directory - Row Column (Company Name)");
        public static String Search_Directory_Organization_ColumnCityState = AutoDefine("Search Directory - Row Column (City State)");
        public static String Search_Directory_Organization_ColumnAddress = AutoDefine("Search Directory - Row Column (Address)");
        public static String Search_Directory_Organization_ColumnCity = AutoDefine("Search Directory - Row Column (City)");
        public static String Search_Directory_Organization_ColumnState = AutoDefine("Search Directory - Row Column (State)");
        public static String Search_Directory_Organization_ColumnCountry = AutoDefine("Search Directory - Row Column (Country)");
        public static String Search_Directory_Organization_ColumnCreditScore = AutoDefine("Search Directory - Row Column (Credit Score)");
        public static String Search_Directory_Organization_ColumnComplianceAndMedia = AutoDefine("Search Directory - Row Column (Compliance and Media)");
        public static String Search_Directory_Organization_Company = AutoDefine("Search Directory - Company Value");
        public static String Search_Directory_Organization_Address = AutoDefine("Search Directory - Address Value");
        public static String Search_Directory_Organization_City = AutoDefine("Search Directory - City Value");
        public static String Search_Directory_Organization_State = AutoDefine("Search Directory - State Value");
        public static String Search_Directory_Organization_Country = AutoDefine("Search Directory - Country Value");
        public static String Search_Directory_Organization_Status = AutoDefine("Search Directory - Status Value");
        public static String Search_Directory_Organization_CreditScore_Icon_Lock = AutoDefine("Search Directory Credit Score Icon - Lock");
        public static String Search_Directory_Organization_CreditScore = AutoDefine("Search Directory - Credit Score Value");
        public static String Search_Directory_Compliance_Icon_Loading_Spinner = AutoDefine("Search Directory Compliance Icon - Loading Spinner");
        public static String Search_Directory_Compliance_Icon_Lock = AutoDefine("Search Directory Compliance Icon - Lock");
        public static String Search_Directory_Compliance_Icon_Question = AutoDefine("Search Directory Compliance Icon - Question Mark");
        public static String Search_Directory_Compliance_Icon_Triangle = AutoDefine("Search Directory Compliance Icon - Triangle");
        public static String Search_Directory_Compliance_Icon_Exclamation = AutoDefine("Search Directory Compliance Icon - Exclamation Mark");
        public static String Search_Directory_Compliance_Icon_Ban = AutoDefine("Search Directory Compliance Icon - Ban");
        public static String SearchDirectory_TableColumn_Selected = AutoDefine("Search Directory Table Column Selected");
        public static String Search_Directory_Compliance_Icon_Alerts = AutoDefine("Search Directory Compliance Icon - Alerts");
        #endregion

        #region Root Layout
        // Root Layout
        public static String Navigation_CompanyProfile = AutoDefine("Navigation - Company Profile");
        public static String Navigation_CompanyProfile_NewRecordRequest = AutoDefine("Navigation - Company Profile - New Record Request");
        #endregion

        #region Users Page
        public static String UsersPage_InviteModal_InviteError = AutoDefine("Users Page - Invite Modal (Invite Error)");
        public static String UsersPage_InviteModal_InviteSuccess = AutoDefine("Users Page - Invite Modal (Invite Success)");
        public static String UsersPage_InviteModal_FirstNameInput = AutoDefine("Users Page - Invite Modal (First Name Input)");
        public static String UsersPage_InviteModal_LastNameInput = AutoDefine("Users Page - Invite Modal (Last Name Input)");
        public static String UsersPage_InviteModal_EmailInput = AutoDefine("Users Page - Invite Modal (Email Input)");
        public static String UsersPage_InviteModal_CloseTopLeftButton = AutoDefine("Users Page - Invite Modal (Close Top Left Button)");
        public static String UsersPage_InviteModal = AutoDefine("Users Page - Invite Modal");
        public static String UsersPage_InviteModal_CloseButton = AutoDefine("Users Page - Invite Modal (Close Button)");
        public static String UsersPage_InviteModal_CancelButton = AutoDefine("Users Page - Invite Modal (Cancel Button)");
        public static String UsersPage_InviteModal_InviteButton = AutoDefine("Users Page - Invite Modal (Invite Button)");
        public static String UsersPage_InviteLink = AutoDefine("Users Page - Invite Link");

        #endregion

        #region New Record Request

        // New Record Request Page
        public static String Navigation_AdminSection_DataStewartQueueLink = AutoDefine("Navigation - New Record Request");
        public static String NewRecordRequest_Table = AutoDefine("New Record Request - Table");
        public static String NewRecordRequest_TableRow = AutoDefine("New Record Request - Table Row");
        public static String NewRecordRequest_TableColumn_RecordId = AutoDefine("New Record Request - Table Column (Record ID)");
        public static String NewRecordRequest_TableColumn_State = AutoDefine("New Record Request - Table Column (State)");
        public static String NewRecordRequest_TableColumn_Date = AutoDefine("New Record Request - Table Column (Date)");
        public static String NewRecordRequest_DetailsModal = AutoDefine("New Record Request - Details (Modal)");
        public static String NewRecordRequest_DetailsModal_Approve = AutoDefine("New Record Request - Details Modal - Approve");
        public static String NewRecordRequest_DetailsModal_Reject = AutoDefine("New Record Request - Details Modal - Reject");
        public static String NewRecordRequest_ConfirmModal= AutoDefine("New Record Request - Confirm Modal");
        public static String NewRecordRequest_ConfirmModal_OrgIdInput = AutoDefine("New Record Request - Confirm Modal (Org Id Input)");
        public static String NewRecordRequest_ConfirmModal_Confirm = AutoDefine("New Record Request - Confirm Modal (Confirm)");

        public static String NewRecordRequest_TableNoRecords = AutoDefine("New Record Request - Table No Records");
        public static String NewRecordRequest_DetailsModal_Cancel = AutoDefine("New Record Request - Details Modal (Cancel)");
        public static String NewRecordRequest_ConfirmModal_Cancel = AutoDefine("New Record Request - Confirm Modal (Cancel)");
        public static String NewRecordRequest_TableErrorFound = AutoDefine("New Record Request - Table Error Found");
        public static String NewRecordRequest_TableColumn_CompanyName = AutoDefine("New Record Request - Table Column (Company Name)");
        public static String NewRecordRequest_TableColumn_RequestType = AutoDefine("New Record Request - Table Column (Request Type)");
        public static String NewRecordRequest_TableRow_Link = AutoDefine("New Record Request - Table Row (Link)");
        public static String NewRecordRequest_RejectModal = AutoDefine("New Record Request - Reject Modal");
        public static String NewRecordRequest_RejectModal_Reason = AutoDefine("New Record Request - Reject Modal (Reason)");
        public static String NewRecordRequest_ConfirmModal_Reason = AutoDefine("New Record Request - Confirm Modal (Reason)");
        public static String NewRecordRequest_ConfirmModal_RequestStatus = AutoDefine("New Record Request - Confirm Modal (Request Status");
        public static String SearchDirectory_AdvancedSearchButton = AutoDefine("Search Directory - Advanced Search Button");
        public static String SearchDirectory_CompanyNameInput = AutoDefine("Search Directory - Company Name Input");
        public static String LayoutPage_SettingsLink = AutoDefine("Layout Page - Settings Link");
        public static String LayoutPage_SettingsDropdown = AutoDefine("Layout Page - Settings Dropdown");
        public static String LayoutPage_SettingsDropdown_Items = AutoDefine("Layout Page - Settings Dropdown (Items)");
        public static String NewRecordRequest_TableColumn_QueueState = AutoDefine("New Record Request - Table Column (Queue Status)");
        public static String NewRecordRequest_TableColumn_ModifiedByUser = AutoDefine("New Record Request - Table Column (Modified By User)");
        public static String NewRecordRequest_TableColumn_ModifiedDate = AutoDefine("New Record Request - Table Column (Modified Date)");
        public static String NewRecordRequest_TableHeader_CompanyName = AutoDefine("New Record Request - Table Header (Company Name)");
        public static String NewRecordRequest_TableHeader_Date = AutoDefine("New Record Request - Table Header (Date)");
        public static String NewRecordRequest_TableHeader_RequestType = AutoDefine("New Record Request - Table Header (Request Type)");
        public static String NewRecordRequest_TableHeaderRow = AutoDefine("New Record Request - Table Header Row");
        public static String NewRecordRequest_TableHeader_QueueState = AutoDefine("New Record Request - Table Header (Queue State)");
        public static String NewRecordRequest_TableHeader_DispositionStatus = AutoDefine("New Record Request - Table Header (Disposition Status)");
        public static String NewRecordRequest_TableHeader_UpdatedBy = AutoDefine("New Record Request - Table Header (Updated By)");
        public static String NewRecordRequest_TableHeader_UpdatedDate = AutoDefine("New Record Request - Table Header (Updated Date)");

        #endregion

        #region CounterParty Elements

        public static String CounterPartyFilePopup_QueueKey = AutoDefine("Counter Party File Popup - Queue Key");
        public static String CounterPartyFilePopup_MatchScore = AutoDefine("Counter Party File Popup - Match Score");
        public static String CounterPartyFilePopup_Condition = AutoDefine("Counter Party File Popup - Condition");
        public static String CounterPartyFilePopup_RegisteredBusinessName = AutoDefine("Counter Party File Popup - Registered Business Name");
        public static String CounterPartyFilePopup_StreetAddress = AutoDefine("Counter Party File Popup - Street Address");
        public static String CounterPartyFilePopup_AddressLine1 = AutoDefine("Counter Party File Popup - Address Line 1");
        public static String CounterPartyFilePopup_AddressLine2 = AutoDefine("Counter Party File Popup - Address Line 2");
        public static String CounterPartyFilePopup_AddressLine3 = AutoDefine("Counter Party File Popup - Address Line 3");
        public static String CounterPartyFilePopup_City = AutoDefine("Counter Party File Popup - City");
        public static String CounterPartyFilePopup_State = AutoDefine("Counter Party File Popup - State");
        public static String CounterPartyFilePopup_Country = AutoDefine("Counter Party File Popup - Country");
        public static String CounterPartyFilePopup_Zip = AutoDefine("Counter Party File Popup - Zip");
        public static String CounterPartyFilePopup_Phone = AutoDefine("Counter Party File Popup - Phone");
        public static String CounterPartyFilePopup_EIN = AutoDefine("Counter Party File Popup - EIN");
        public static String CounterPartyFilePopup_TIN = AutoDefine("Counter Party File Popup - TIN");
        public static String CounterPartyFilePopup_VAT = AutoDefine("Counter Party File Popup - VAT");
        public static String CounterPartyFilePopup_RegistrationNumber = AutoDefine("Counter Party File Popup - Registration Number");
        public static String CounterPartyFilePopup_StreetAddress2 = AutoDefine("Counter Party File Popup - Street Address 2");
        public static String CounterPartyFilePopup_City3 = AutoDefine("Counter Party File Popup - City 3");
        public static String CounterPartyFilePopup_State3 = AutoDefine("Counter Party File Popup - State 3");
        public static String CounterPartyFilePopup_Country3 = AutoDefine("Counter Party File Popup - Country 3");
        public static String CounterPartyFilePopup_Zip3 = AutoDefine("Counter Party File Popup - Zip 3");
        public static String CounterPartyFilePopup_CompanyInformation = AutoDefine("Counter Party File Popup - Company Information");
        public static String CounterPartyFilePopup_ContactInformation = AutoDefine("Counter Party File Popup - Contract Information");
        public static String CounterPartyFilePopup_Information = AutoDefine("Counter Party File Popup - Information");

        public static String CounterPartyFilePopup_DropDown_Source = AutoDefine("Counter Party File Popup - Dropdown (Source)");
        public static String CounterPartyFilePopup_DropDown_Source_ReadOnly = AutoDefine("Counter Party File Popup - Dropdown (Source) -- Readonly");
        public static String CounterPartyFilePopup_TextBox_Source = AutoDefine("Counter Party File Popup - Textbox (Source)");
        public static String CounterPartyFilePopup_TextBox_Source_ReadOnly = AutoDefine("Counter Party File Popup - Textbox (Source) -- Readonly");
        public static String CounterPartyFilePopup_DropDown_Disposition = AutoDefine("Counter Party File Popup - Dropdown Disposition");
        public static String CounterPartyFilePopup_DropDown_Disposition_Readonly = AutoDefine("Counter Party File Popup - Dropdown Disposition -- Readonly");
        public static String CounterPartyFilePopup_CheckBox_AddAlias = AutoDefine("Counter Party File Popup - Checkbox (Add Alias)");
        public static String CounterPartyFilePopup_Button_Save = AutoDefine("Counter Party File Popup - Button Save");
        public static String CounterPartyFilePopup_TextBox_Zip = AutoDefine("Counter Party File Popup - TextBox Zip");
        public static String CounterPartyFilePopup_DropDown_Country = AutoDefine("Counter Party File Popup - Dropdown (Country)");
        public static String CounterPartyFilePopup_TextBox_Address = AutoDefine("Counter Party File Popup - TextBox (Address)");
        public static String CounterPartyFilePopup_TextBox_City = AutoDefine("Counter Party File Popup - TextBox (City)");
        public static String CounterPartyFilePopup_DropDown_State = AutoDefine("Counter Party File Popup - Dropdown (State)");

        public static String CounterpartyPage_UploadedFilesTable = AutoDefine("Counter Party Page - Uploaded Files Table");
        public static String CounterpartyPage_UploadedFilesTable_Column_Actions = AutoDefine("Counter Party Page - Uploaded Files Table (Column) - Actions");
        public static String CounterpartyPage_UploadedFilesTable_Column_Actions_Item = AutoDefine("Counter Party Page - Uploaded Files Table (Column) - Actions Item");
        public static String CounterpartyPage_UploadedFilesTable_Column_FileName = AutoDefine("Counter Party Page - Uploaded Files Table (Column) - File Name");
        public static String CounterpartyPage_UploadedFilesTable_Column_LastNameFirstName = AutoDefine("Counter Party Page - Uploaded Files Table (Column) - Last Name First Name");
        public static String CounterpartyPage_UploadedFilesTable_Column_Status = AutoDefine("Counter Party Page - Uploaded Files Table (Column) - Status");
        public static String CounterpartyPage_UploadedFilesTable_Column_UploadDate = AutoDefine("Counter Party Page - Uploaded Files Table (Column) - Upload Date");
        public static String CounterpartyPage_UploadedFilesTable_Row = AutoDefine("Counter Party Page - Uploaded Files Table (Row)");

        #endregion

        #region No Match Modal

        // C6 No Match Modal
        public static String C6NoMatch_Disposition_Dropdown = AutoDefine("C6 No Match Popup - Dropdown (Disposition)");
        public static String C6NoMatch_Source_DropDown = AutoDefine("C6 No Match Popup - Dropdown (Source)");
        public static String C6NoMatch_SourceId_Input = AutoDefine("C6 No Match Popup - Source Id Input");
        public static String C6NoMatch_Source_DropDown_ReadOnly = AutoDefine("C6 No Match Popup - Dropdown (Source) -- Readonly");
        public static String C6NoMatch_SourceId_Input_ReadOnly = AutoDefine("C6 No Match Popup - Source Id Input -- Readonly");
        public static String C6NoMatch_AliasAddress_CheckBox = AutoDefine("C6 No Match Popup - Alias Address (Checkbox)");
        public static String C6NoMatch_AliasAddress_CheckBox_ReadOnly = AutoDefine("C6 No Match Popup - Alias Address (Checkbox) -- Readonly");
        public static String C6NoMatch_AliasAddress_DisplayWindow = AutoDefine("C6 No Match Popup - Display Window");
        public static String C6NoMatch_Save_Button = AutoDefine("C6 No Match Popup - Save Button");
        public static String C6NoMatch_ValidatingCompany_ValidationMessage = AutoDefine("C6 No Match Popup - Validating Company (Validation Message)");
        public static String C6NoMatch_CompanyFound_ValidationMessage = AutoDefine("C6 No Match Popup - Company Found - Validation Message");
        public static String C6NoMatch_NoCompanyFound_ValidationMessage = AutoDefine("C6 No Match Popup - No Company Found - Validation Message");
        public static String C6NoMatch_AliasAddressWindow_Add1 = AutoDefine("C6 No Match Popup - Alias Address Window (Address 1)");
        public static String C6NoMatch_AliasAddressWindow_Add2 = AutoDefine("C6 No Match Popup - Alias Address Window (Address 2)");
        public static String C6NoMatch_AliasAddressWindow_City = AutoDefine("C6 No Match Popup - Alias Address Window (City)");
        public static String C6NoMatch_AliasAddressWindow_State = AutoDefine("C6 No Match Popup - Alias Address Window (State)");
        public static String C6NoMatch_AliasAddressWindow_Zip = AutoDefine("C6 No Match Popup - Alias Address Window (Zip)");
        public static String C6NoMatch_ValidateOverwrite_ConfirmationModal = AutoDefine("C6 No Match Popup - Validate Overwrite (Confirmation Modal)");
        public static String C6NoMatch_ValidateOverwrite_ContinueButton = AutoDefine("C6 No Match Popup - Validate Overwrite (Contiute Button)");
        public static String C6NoMatch_ValidateOverwrite_CancelButton = AutoDefine("C6 No Match Popup - Validate Overwrite (Cancel Button)");
        public static String C6NoMatch_ValidateOverwrite_CompanyNameField = AutoDefine("C6 No Match Popup - Validate Overwrite (Company Name Field)");
        public static String C6NoMatch_ValidateOverwrite_C6UniqueIdField = AutoDefine("C6 No Match Popup - Validate Overwrite (C6 Unique Id Field)");
        public static String C6NoMatch_Close_Button = AutoDefine("C6 No Match Popup - Validate Overwrite (Close Button)");

        #endregion

        #region Detail Directive

        // Detail Directive
        public static String C6NoMatch_Detail_RequestID = AutoDefine("C6 No Match Popup - Details (Request ID)");
        public static String C6NoMatch_Detail_CompanyName = AutoDefine("C6 No Match Popup - Details (Company Name)");
        public static String C6NoMatch_Detail_CompanyID = AutoDefine("C6 No Match Popup - Details (Company ID)");
        public static String C6NoMatch_Detail_Address1 = AutoDefine("C6 No Match Popup - Details (Address1)");
        public static String C6NoMatch_Detail_Address2 = AutoDefine("C6 No Match Popup - Details (Address2)");
        public static String C6NoMatch_Detail_City = AutoDefine("C6 No Match Popup - Details (City)");
        public static String C6NoMatch_Detail_State = AutoDefine("C6 No Match Popup - Details (State)");
        public static String C6NoMatch_Detail_Country = AutoDefine("C6 No Match Popup - Details (Country)");
        public static String C6NoMatch_Detail_Zip = AutoDefine("C6 No Match Popup - Details (Zip)");
        public static String C6NoMatch_Detail_CompanyPhone = AutoDefine("C6 No Match Popup - Details (Company Phone");
        public static String C6NoMatch_Detail_CompanyURL = AutoDefine("C6 No Match Popup - Details (Company URL)");

        #endregion

        #region My Files UI Elements

        //My Files Page
        public static String MatchInsights_Subscription_Modal = AutoDefine("Match Insights - Subscription MOdal");
        public static String MatchInsights_Insights_Highlight = AutoDefine("Match Insights - Highlight");
        public static String MatchInsights_HighConfidenceMatch_Highlight = AutoDefine("Match Insights - High Confidence Match (Highlight)");
        public static String MatchInsights_LikelyMatch_Highlight = AutoDefine("Match Insights - Likely Match (Highlight)");
        public static String MatchInsights_NoMatch_Highlight = AutoDefine("Match Insights - No Match (Highlight)");
        public static String MyFiles_SubscriptionModal_PurchaseButton = AutoDefine("My Files - Subscription Modal (Purchase Button)");
        public static String MyFiles_SubscriptionModal_CancelButton = AutoDefine("My Files - Subscription Modal (Cancel Button)");

        //Confirmation dialog
        public static String Confirmation_Dialog_Header = AutoDefine("Confirmation Dialog - Header Text");
        public static String Confirmation_Dialog_Body = AutoDefine("Confirmation Dialog - Body Text");
        public static String Confirmation_Dialog_Action = AutoDefine("Confirmation Dialog - Action Button");
        public static String AdminSearchResults_ConfirmAdminViewModal_Footer_GoToCompanyButton = AutoDefine("AdminSearchResults_ConfirmAdminViewModal_Footer_GoToCompanyButton");
        public static String Confirmation_Dialog_Close = AutoDefine("Confirmation Dialog - Close Button");
        public static String Confirmation_Dialog_topXClose = AutoDefine("Confirmation Dialog - Top Close Button");


        #endregion

        #region My Purchases UI Elements
        //Purchase/UnPurchase
        public static String HighConfidence_PurchasePremiumMonitoring_Button = AutoDefine("HighConfidence-Purchase Premium Monitoring Button");
        public static String HighConfidence_UnsubscribePremiumMonitoring_Button = AutoDefine("HighConfidence-Unsubscribe Premium Monitoring Button");
        public static String CompanyProfile_PurchasePremiumMonitoring_Button = AutoDefine("CompanyProfile-Premium Purchase Monitoring Button");

        public static String MyFiles_Global_Record_Profile_Chart = AutoDefine("My Files - Global Record Profile Chart");
        public static String MyFiles_Input_File_Quality_Chart = AutoDefine("My Files - Input File Quality Chart");
        // My Purchases Page
        public static String MyPurchases_Table = AutoDefine("My Purchase Page - Table");
        public static String MyPurchases_TableHeaderRow = AutoDefine("My Purchase Page - Table Header Row");
        public static String MyPurchases_TableHeader_CompanyName = AutoDefine("My Purchase Page - Table Header (Company Name)");
        public static String MyPurchases_TableHeader_StreetAddress = AutoDefine("My Purchase Page - Table Header (Street Address)");
        public static String MyPurchases_TableHeader_City = AutoDefine("My Purchase Page - Table Header (City)");
        public static String MyPurchases_TableHeader_StateProvince = AutoDefine("My Purchase Page - Table Header (State Province)");
        public static String MyPurchases_TableHeader_Country = AutoDefine("My Purchase Page - Table Header (Country)");
        public static String MyPurchases_TableHeader_Zip = AutoDefine("My Purchase Page - Table Header (Zip)");
        public static String MyPurchases_TableHeader_Status = AutoDefine("My Purchase Page - Table Header (Status)");
        public static String MyPurchases_TableHeader_CreditScore = AutoDefine("My Purchase Page - Table Header (Credit Score)");
        public static String MyPurchases_TableHeader_PurchasedDate = AutoDefine("My Purchase Page - Table Header (Purchase Date)");
        public static String MyPurchases_Sub_TableHeaderRow = AutoDefine("My Purchase Page - Table Sub Header Row");
        public static String MyPurchases_TableHeader_Latest = AutoDefine("My Purchase Page - Table Header (Latest)");
        public static String MyPurchases_TableHeader_TotalAlerts = AutoDefine("My Purchase Page - Table Header (Total Alerts)");
        public static String MyPurchases_TableFilterRow = AutoDefine("My Purchase Page - Table Filter Row");
        public static String MyPurchases_TableFilter_City = AutoDefine("My Purchase Page - Table Filter Column (City)");
        public static String MyPurchases_TableFilter_StateProvince = AutoDefine("My Purchase Page - Filter Column (State Province)");
        public static String MyPurchases_TableFilter_Country = AutoDefine("My Purchase Page - Table Filter Column (Country)");
        public static String MyPurchases_TableFilter_Zip = AutoDefine("My Purchase Page - Table Filter Column (Zip)");
        public static String MyPurchases_TableFilter_Status = AutoDefine("My Purchase Page - Table Filter Column (Status)");
        public static String MyPurchases_TableFilter_CreditScore = AutoDefine("My Purchase Page - Table Filter Column (Credit Score)");
        public static String MyPurchases_TableFilter_Latest = AutoDefine("My Purchase Page - Table Filter Column (Latest)");
        public static String MyPurchases_TableFilter_PurchasedDate = AutoDefine("My Purchase Page - Table Filter Column (Purchase Date)");
        public static String MyPurchases_TableRow = AutoDefine("My Purchase Page - Table Row");
        public static String MyPurchases_TableColumn_CompanyName = AutoDefine("My Purchase Page - Table Column (Company Name)");
        public static String MyPurchases_TableColumn_StreetAddress = AutoDefine("My Purchase Page - Table Column (Street Address)");
        public static String MyPurchases_TableColumn_City = AutoDefine("My Purchase Page - Table Column (City)");
        public static String MyPurchases_TableColumn_StateProvince = AutoDefine("My Purchase Page - Table Column (State Province)");
        public static String MyPurchases_TableColumn_Country = AutoDefine("My Purchase Page - Table Column (Country)");
        public static String MyPurchases_TableColumn_Zip = AutoDefine("My Purchase Page - Table Column (Zip)");
        public static String MyPurchases_TableColumn_Status = AutoDefine("My Purchase Page - Table Column (State)");
        public static String MyPurchases_TableColumn_CreditScore = AutoDefine("My Purchase Page - Table Column (Credit Score)");
        public static String MyPurchases_TableColumn_Latest = AutoDefine("My Purchase Page - Table Column (Latest)");
        public static String MyPurchases_TableColumn_TotalAlerts = AutoDefine("My Purchase Page - Table Column (Total Alerts)");
        public static String MyPurchases_TableColumn_PurchasedDate = AutoDefine("My Purchase Page - Table Column (Purchased Date)");
        public static String MyPurchases_TableNoRecords = AutoDefine("My Purchase Page - Table (No Records)");
        public static String MyPurchases_UnSubscribeButton = AutoDefine("Unsubscribe to Premium Monitoring Button");
        public static String MyPurchases_UnsubscribeSuccessClose = AutoDefine("Unsubscribe Sucess Dialog - Close");
        #endregion

        #region Compliance UI Elements

        public static String Compliance_Modal_Detail_Tab = AutoDefine("Compliance - Details Tab");
        public static String Compliance_Modal_Detail_Show_Alerts = AutoDefine("Compliance - Show/Hide Compliance Alerts");
        public static String Compliance_Modal_Detail_Alerts = AutoDefine("Compliance - Compliance Alerts - Company Linked");
        public static String Compliance_Modal_Detail_Alert_Type = AutoDefine("Compliance - Compliance Alert Type");
        public static String Compliance_Modal_Detail_Alert_Number = AutoDefine("Compliance - Compliance Alert Count");
        public static String Compliance_Modal_Detail_No_Alerts = AutoDefine("Compliance - No Compliance Alerts Loaded");
        public static String Compliance_Modal_Detail_Hide_Alerts = AutoDefine("Compliance - Compliance Alerts - Company Unlinked");
        public static String Compliance_Modal_Detail_Show_Aliases = AutoDefine("Compliance - Display Aliases");
        public static String Compliance_Modal_Detail_Alias = AutoDefine("Compliance - Alias Table Item");
        public static String Compliance_Modal_Detail_Aliases_Table = AutoDefine("Compliance - Aliases Table");
        public static String Compliance_Modal_Detail_Aliases_TableRow = AutoDefine("Compliance - Aliases Table Row");
        public static String Compliance_Modal_Detail_Show_Addresses = AutoDefine("Compliance - Display Addresses");
        public static String Compliance_Modal_Detail_Addresses_Table = AutoDefine("Compliance - Addresses Table");
        public static String Compliance_Modal_Detail_Addresses_TableRow = AutoDefine("Compliance - Addresses Table Row");
        public static String Compliance_Modal_Detail_Addresses = AutoDefine("Compliance - Address Table Item");
        public static String Compliance_Modal_Detail_Show_Notes = AutoDefine("Compliance - Display Notes");
        public static String Compliance_Modal_Detail_Notes_Panel = AutoDefine("Compliance - Notes Panel");
        public static String Compliance_Modal_Detail_Notes = AutoDefine("Compliance - Notes Element");
        public static String Compliance_Modal_Detail_Hide_Notes = AutoDefine("Compliance - Show/Hide Notes");
        public static String Compliance_Modal_Detail_Show_Reference_ID = AutoDefine("Compliance - Display Reference ID");
        public static String Compliance_Modal_Detail_ReferenceID = AutoDefine("Compliance - Reference ID Element");

        public static String Compliance_Modal_Linked_Businesses_Tab = AutoDefine("Compliance - Linked Businesses Tab");
        public static String Compliance_Modal_Linked_Businesses_Show_Businesses = AutoDefine("Compliance - Show/Hide Linked Businesses");
        public static String Compliance_Modal_Linked_Businesses_Business_Name = AutoDefine("Compliance - Businesses - Display Business Name");
        public static String Compliance_Modal_Linked_Businesses_Business_Connection = AutoDefine("Compliance - Businesses - Display Business Connection");

        public static String Compliance_Modal_Linked_Persons_Tab = AutoDefine("Compliance - Linked Persons Tab");
        public static String Compliance_Modal_Linked_Persons_Show_Persons = AutoDefine("Compliance - Show/Hide Linked Persons");
        public static String Compliance_Modal_Linked_Persons_Person_Name = AutoDefine("Compliance - Persons - Display Person Name");
        public static String Compliance_Modal_Linked_Persons_Person_Position = AutoDefine("Compliance - Persons - Display Person Position");
        public static String Compliance_Modal_Linked_Persons_Person_Compliance_Type = AutoDefine("Compliance - Persons - Display Person Compliance Time");

        public static String ComplianceDocuments_Compliance_Select_Filter = AutoDefine("Compliance - Documents Filter");
        public static String ComplianceDocuments_Table = AutoDefine("Compliance - Documents Table");
        public static String ComplianceDocuments_TableHeader_Row = AutoDefine("Compliance - Documents Table Header Row");
        public static String ComplianceDocuments_TableHeader_Name = AutoDefine("Compliance - Documents Table Header (Name)");
        public static String ComplianceDocuments_TableHeader_Categories = AutoDefine("Compliance - Documents Table Header (Categories)");
        public static String ComplianceDocuments_TableHeader_CreationDate = AutoDefine("Compliance - Documents Table Header (Creation Date)");
        public static String ComplianceDocuments_TableBody_Row = AutoDefine("Compliance - Documents Table Body Row");
        public static String ComplianceDocuments_TableBody_Name = AutoDefine("Compliance - Documents Table Body (Name)");
        public static String ComplianceDocuments_TableBody_Categories = AutoDefine("Compliance - Documents Table Body (Categories)");
        public static String ComplianceDocuments_TableBody_CreationDate = AutoDefine("Compliance - Documents Table Body (Creation Date)");
        public static String ComplianceDocuments_TableNoRecords = AutoDefine("Compliance - Documents Table No Records Found");
        public static String ComplianceDocuments_Pagination_ControlSet = AutoDefine("Compliance - Documents Pagination");
        public static String Pagination_ControlSet = AutoDefine("Pagination - Control Set");
        #endregion

        #region High Confidence
        public static String High_Confidence_Table = AutoDefine("High Confidence Match - Table");
        public static String High_Confidence_Table_Row = AutoDefine("High Confidence Match - Table Row");
        // High Confidence - Input
        public static String High_Confidence_SupplierID = AutoDefine("High Confidence Match - Supplier ID");
        public static String High_Confidence_Input_Company_Name = AutoDefine("High Confidence Match - Company Name");
        public static String High_Confidence_Input_Address = AutoDefine("High Confidence Match - Address");
        public static String High_Confidence_Input_City = AutoDefine("High Confidence Match - City");
        public static String High_Confidence_Input_State = AutoDefine("High Confidence Match - State");
        public static String High_Confidence_Input_Country = AutoDefine("High Confidence Match - Country");
        // High Confidence - Output
        public static String High_Confidence_Output_Company_Name = AutoDefine("High Confidence Match - Country");
        public static String High_Confidence_Output_Address = AutoDefine("High Confidence Match - Address");
        public static String High_Confidence_Output_City = AutoDefine("High Confidence Match - City");
        public static String High_Confidence_Output_State = AutoDefine("High Confidence Match - State");
        public static String High_Confidence_Output_Country = AutoDefine("High Confidence Match - Country");
        public static String High_Confidence_Output_Percentage = AutoDefine("High Confidence Match - Percentage");
        public static String High_Confidence_Output_Status = AutoDefine("High Confidence Match - Status");
        #endregion

        #region Company Profile
        // Company Profile
        public static String Company_Profile_Page = AutoDefine("Company Profile Page");
        public static String Company_Profile_Page_Details_Compliance_And_Media = AutoDefine("Company Profile Page - Compliance And Media Section");
        #endregion

        #region Credit Score UI Element
        public static String CreditScore_Root = AutoDefine("Credit Score Directive - Root");
        public static String CreditScore_Loading = AutoDefine("Credit Score Directive - Loading");
        public static String CreditScore_UnsuccessfulLoad = AutoDefine("Credit Score Directive - Unsuccessful Load");
        public static String CreditScore = AutoDefine("Credit Score Directive - Credit Score");
        public static String CreditScore_Locked = AutoDefine("Credit Score Directive - Locked");

        #endregion

        #region Acuris Copyright
        public static String AcurisCopyright_Logo = AutoDefine("Acuris Logo");
        public static String AcurisCopyright_Notice = AutoDefine("Acuris Notice");
        #endregion

        #endregion
    }
}


