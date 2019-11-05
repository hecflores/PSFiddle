using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Services;
using AE.Net.Mail;
using MC.Track.TestSuite.Model.Domain;
using Jane;
using MC.Track.TestSuite.Model.Helpers;

namespace MC.Track.TestSuite.Services.Services
{
    public class EmailService : IEmailService
    {
        private Dictionary<String, Action<EmailConfiguration>> EmailConfigurations = new Dictionary<string, Action<EmailConfiguration>>()
        {
            {
                "outlook.com" ,
                (config) =>
                {
                    config.POPServer = "imap-mail.outlook.com";
                    config.IncomingPort = "993";
                    config.IsPOPssl = true;
                }
            },
            {
                "service.microsoft.com" ,
                (config) =>
                {
                    config.POPServer = "outlook.office365.com";
                    config.IncomingPort = "993";
                    config.IsPOPssl = true;
                }
            }
        };
        public IResult DeleteAllMessages(String UserName, String Password)
        {
            var inbox = new List<MailMessage>();
            try
            {
                var domain = Regex.Replace(UserName, @".*\@(.*)", "$1");
                if (!EmailConfigurations.ContainsKey(domain))
                    throw new Exception($"No email configurations available for email domain {domain}");
                
                EmailConfiguration email = new EmailConfiguration();
                EmailConfigurations[domain](email);
                email.POPUsername = UserName; // type your username credential
                email.POPpassword = Password; // type your password credential
                

                using (ImapClient ic = new ImapClient(email.POPServer, email.POPUsername, email.POPpassword, AuthMethods.Login, Convert.ToInt32(email.IncomingPort), (bool)email.IsPOPssl))
                {
                    Action<String> mainAction = (inboxType) =>
                    {
                        // Select a mailbox. Case-insensitive
                        ic.SelectMailbox(inboxType);
                        int i = 1;
                        int msgcount = ic.GetMessageCount(inboxType);
                        int end = msgcount - 1;
                        int start = msgcount - msgcount;
                        if (msgcount > 0)
                        {
                            // Note that you must specify that headersonly = false
                            // when using GetMesssages().
                            MailMessage[] mm = ic.GetMessages(start, end, false);
                            // var messages = ic.GetMessages(start, end, true);
                            foreach (var it1 in mm)
                            {
                                ic.DeleteMessage(it1);
                            }
                        }
                    };
                    mainAction("JUNK");
                    mainAction("INBOX");
                }



                return Result.Success();

            }

            catch (Exception e)
            {
                XConsole.WriteLine($"Delete all Emails failed for user {UserName}:\n{e.Message}\n{e.StackTrace}\n");
                return Result.Failure(e);
            }
        }

        public IResult<List<MailMessage>> ReceiveMails(String UserName, String Password)
        {

            var inbox = new List<MailMessage>();
            try
            {
                var domain = Regex.Replace(UserName, @".*\@(.*)", "$1");
                if (!EmailConfigurations.ContainsKey(domain))
                    throw new Exception($"No email configurations available for email domain {domain}");

                EmailConfiguration email = new EmailConfiguration();
                EmailConfigurations[domain](email);
                email.POPUsername = UserName; // type your username credential
                email.POPpassword = Password; // type your password credential
                int success = 0;
                int fail = 0;

                using (ImapClient ic = new ImapClient(email.POPServer, email.POPUsername, email.POPpassword, AuthMethods.Login, Convert.ToInt32(email.IncomingPort), (bool)email.IsPOPssl))
                {
                    Action<String> mainAction = (inboxType) =>
                    {
                        // Select a mailbox. Case-insensitive
                        ic.SelectMailbox(inboxType);
                        int i = 1;
                        int msgcount = ic.GetMessageCount(inboxType);
                        int end = msgcount - 1;
                        int start = msgcount - msgcount;
                        if (msgcount > 0)
                        {
                            // Note that you must specify that headersonly = false
                            // when using GetMesssages().
                            MailMessage[] mm = ic.GetMessages(start, end, false);
                            inbox.AddRange(mm);
                            // var messages = ic.GetMessages(start, end, true);
                        }
                    };
                    mainAction("INBOX");
                    mainAction("JUNK");
                }



                return Result.Success<List<MailMessage>>(inbox);

            }

            catch (Exception e)
            {
                XConsole.WriteLine($"Receive Emails failed for user {UserName}:\n{e.Message}\n{e.StackTrace}\n");
                return Result.Failure<List<MailMessage>>(e);
            }
        }
    }
}
