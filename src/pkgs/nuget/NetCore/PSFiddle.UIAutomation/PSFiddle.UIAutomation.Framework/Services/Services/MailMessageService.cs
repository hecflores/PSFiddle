using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Domain;
using AE.Net.Mail;
using MC.Track.TestSuite.Model.Helpers;

namespace MC.Track.TestSuite.Services.Services
{
    public class MailMessageService : IMailMessageService
    {
        /**
         * Needs to be rewritten
         * */
        public string GetLink(MailMessage message)
        {
            var inputString = message.Body;
            Match m;
            string HRefPattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";

            try
            {
                m = Regex.Match(inputString, HRefPattern,
                                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                TimeSpan.FromSeconds(1));
                while (m.Success)
                {
                    //XConsole.WriteLine("Found href " + m.Groups[1] + " at "
                    //   + m.Groups[1].Index);
                    //m = m.NextMatch();
                    return Regex.Replace(m.Groups[1].ToString(), "amp;", "");
                }
                return string.Empty;
            }

            catch (RegexMatchTimeoutException)
            {
                return string.Empty;
                //onsole.WriteLine("The matching operation timed out.");
            }
            //return string.Empty;
        }

        public bool VerifyNotificationEmail(MailMessage item, Dictionary<string, string> verifyValues)
        {
            bool result = true;
            foreach (var verifyValue in verifyValues.ToArray())
            {
                bool currentResult = false;
                String exepected = verifyValue.Value;
                String actual = null;
                String name = null;

                switch (verifyValue.Key.ToString())
                {
                    case "From":
                        currentResult = item.From.Address.ToLower().Contains(verifyValue.Value);
                        exepected = verifyValue.Value;
                        actual = item.From.Address;
                        name = "From Address";

                        break;
                    case "To":
                        currentResult = item.To.First().Address.ToLower().Contains(verifyValue.Value.ToLower());
                        exepected = verifyValue.Value;
                        actual = item.To.First().Address;
                        name = "To Address";

                        break;
                    case "Subject":
                        currentResult = item.Subject.Contains(verifyValue.Value);
                        exepected = verifyValue.Value;
                        actual = item.Subject;
                        name = "Subject";

                        break;
                    case "Body":
                        currentResult = item.Body.Contains(verifyValue.Value);
                        exepected = verifyValue.Value;
                        actual = item.Body;
                        name = "Body";
                        break;
                    case "PhoneNumber":
                        currentResult = item.Body.Contains(verifyValue.Value);
                        exepected = verifyValue.Value;
                        actual = item.Body;
                        name = "Body";
                        break;

                    default:
                        name = $"Check Key {verifyValue.Key}";
                        currentResult = false;
                        exepected = "Found";
                        actual = "Not Found";
                        break;

                }
                if (currentResult)
                    XConsole.WriteLine($"Value for [{name}]({exepected}) == '{actual}' Ok.");
                else
                    XConsole.WriteLine($"Value for [{name}]({exepected}) != '{actual}' Failed. *ERROR*");

                result &= currentResult;
            }
            return result;
        }
    }
}
