using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Toolkit.Extensions
{
    public class Selectors
    {
        private static String Cfg(String Name)
        {
            return ConfigurationManager.AppSettings[Name];
        }

        private static By GetByPath([CallerMemberName] String configName = "")
        {
            return By.XPath(Cfg($"Selectors_{configName}"));
        }

        public static By Alpha_AcceptContent => GetByPath();
        public static By Alpha_I_Agree => GetByPath();
        public static By Alpha_LoginUserName => GetByPath();
        public static By Alpha_LoginButton => GetByPath();
        public static By Alpha_LoginPassword => GetByPath();
        public static By Alpha_PasswordError => GetByPath();
        public static By Alpha_UsernameError => GetByPath();
        public static By Alpha_UseAnother => GetByPath();
        public static By Alpha_SignInWithDifferentAccount => GetByPath();

        public static By AdminNavigationTab => GetByPath();
        public static By TrackDirectoryNavigationTab => GetByPath();
        public static By MySupplierNavigationTab => GetByPath();
        public static By MyBuyerNavigationTab => GetByPath();
        public static By TransactionsNavigationTab => GetByPath();
        public static By SubscriptionsNavigationTab => GetByPath();
        public static By CompanyProfileNavigationTab => GetByPath();
        public static By ReportsNavigationTab => GetByPath();
        public static By Alpha_RequestChangePopup => By.XPath("//*[@id='iLandingViewAction']");
        
        
    }
}
