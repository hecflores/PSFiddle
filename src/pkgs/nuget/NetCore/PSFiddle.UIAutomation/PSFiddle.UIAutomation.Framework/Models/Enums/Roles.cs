using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Enums
{
    public class Roles
    {
        public const String Browser = "Browser";
        public const String APAdmin = "AP - Admin";
        public const String APDirectorManager = "AP - Director / Manager";
        public const String APApprover = "AP - Approver";
        public const String APAssociate = "AP - Associate";
        public const String APPaymentDataManagement = "AP - Payment Data Management";
        public const String APPaymentDataApprover = "AP - Payment Data Approver";
        public const String ARAdmin = "AR - Admin";
        public const String ARDirectorManager = "AR - Director / Manager";
        public const String ARApprover = "AR - Approver";
        public const String ARAssociate = "AR - Associate";
        public const String ARPaymentDataManagement = "AR - Payment Data Management";
        public const String ARPaymentDataApprover = "AR - Payment Data Approver";
        public const String TreasuryCashManagement = "Treasury/Cash Management";
        public const String ProfileManagement = "Profile - Management";
        public const String ProfileApprover = "Profile - Approver";
        public const String PrincipalOwner = "Principal Owner";
        public const String NetworkAdmin = "Network - Admin";
        public const String NetworkDataManagement = "Network - Data Management";
        public const String NetworkDataManagementApprover = "Network - Data Management Approver";
        public const String NetworkCommercialOwner = "Network - Commercial Owner";
        public const String MasterCardAdmin = "MasterCard - Admin";
        public const String MasterCardOperations = "MasterCard - Operations";
        public const String MasterCardCommercialManager = "MasterCard - Commercial Manager";
        public const String MasterCardCompliance = "MasterCard - Compliance";
        public const String MasterCardITOperations = "MasterCard - IT Operations";
        public const String TestRole = "TestRole";
        public const String TestRoleNoEntiltments = "TestRoleNoEntiltments";
        public const String MasterCardDataSteward = "MasterCard - Data Steward";

        public static List<string> GetListOfRoles()
        {
            return typeof(Roles).GetFields().Select(role => (string)role.GetValue(null)).ToList();
        }
    }
}
