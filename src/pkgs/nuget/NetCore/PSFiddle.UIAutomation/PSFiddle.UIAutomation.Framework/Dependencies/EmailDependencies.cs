using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Model.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Toolkit.Extensions;
using MC.Track.TestSuite.Interfaces.Config;
using AE.Net.Mail;
using MC.Track.TestSuite.Interfaces.Services.Functional;
using MC.Track.TestSuite.UI.Types;
using MC.Track.Shared;
using PSFiddle.UIAutomation.Framework.Shared;
using System.IO;
using System.Text.RegularExpressions;
using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Model.Helpers;

namespace MC.Track.TestSuite.Toolkit.Dependencies
{
    public class EmailDependencies : IEmailDependencies
    {
        private readonly IStateManagment stateManagment;
        private readonly IElementDiscovery elementDiscovery;
        private readonly IParameterParser parameterParser;
        private readonly IEmailService emailService;
        private readonly IFileManager fileManager;
        private readonly IFileSaverService fileSaverService;
        private readonly IRunnerDependencies runningDependencies;
        private readonly IMailMessageService mailMessageService;

        public EmailDependencies(IResolver resolver)
        {
            this.stateManagment = resolver.Resolve<IStateManagment>();
            this.elementDiscovery = resolver.Resolve<IElementDiscovery>();
            this.parameterParser = resolver.Resolve<IParameterParser>();
            this.emailService = resolver.Resolve<IEmailService>();
            this.fileManager = resolver.Resolve<IFileManager>();
            this.fileSaverService = resolver.Resolve<IFileSaverService>();
            this.runningDependencies = resolver.Resolve<IRunnerDependencies>();
            this.mailMessageService = resolver.Resolve<IMailMessageService>();
        }

        public String GetFirstLink(MailMessage mailMessage)
        {
            var links = Regex.Matches(mailMessage.Body, @"href=['""](https:\/\/.*?)['""]");
            Assert.IsTrue(links.Count > 0, $"No Links found in email with subject line {mailMessage.Subject}");

            foreach (Match link in links)
            {
                XConsole.WriteLine($"Found link '{link.Groups[1].Value.Replace("amp;", "")}'");
            }

            var someLink = links[0].Groups[1].Value.Replace("amp;", "");
            return someLink;
        }

        public MailMessage VerifySentEmail(UserType user)
        {
            Dictionary<String, String> Validation = new Dictionary<string, string>();
            
            var stateService = this.stateManagment;
            var emailService = this.emailService;
            var mailMessageService = this.mailMessageService;

            MailMessage EmailFound = null;

            // Try to get emails ateast 10 times
            runningDependencies.RetryAction(
                EvaluationAction: () =>
                {
                    // Get Emails
                    var emailsResult = emailService.ReceiveMails(user.Email, user.Password);
                    Assert.IsTrue(emailsResult.Ok);
                    Assert.IsTrue(emailsResult.Value.Count > 0);

                    // Check emails
                    List<MailMessage> emails = emailsResult.Value;

                    foreach (var email in emails)
                    {

                        var validation = mailMessageService.VerifyNotificationEmail(email, Validation);
                        if (!validation) continue;

                        EmailFound = email;
                        break;
                    }

                    // Set Found Email
                    Assert.IsNotNull(EmailFound);
                },
                Title: $"{user.Email} getting email",
                NumberOfRetries: 3,
                DelayBetweenWaits: TimeSpan.FromSeconds(10),
                ExceptionOnEndingFailure: (e) => new Exception($"Failed to find any email in email box for {user.Email}")
            );

            var text = EmailFound.Subject;
            text = text.Substring(0, Math.Min(30, text.Length));
            var file = this.fileManager.GenerateFileName("Emails", text, "html");
            using (var stream = File.CreateText(file))
            {
                Action<String, String> renderSection = (title, body) => {
                    stream.WriteLine("<email-section>");
                    stream.WriteLine($"<email-title>{title}</email-title>");
                    stream.WriteLine($"<email-body>{body}</email-body>");
                    stream.WriteLine("</email-section>");
                };

                stream.WriteLine("<html>");
                stream.WriteLine("<head>");
                stream.WriteLine("<style>");
                stream.WriteLine("email-section{ margin: 10px; display: block; }");
                stream.WriteLine("email-section > email-title{ padding: 10px; border-radius:10px; font-weight:bold; font-size:20px; display: block; }");
                stream.WriteLine("email-section > email-body{ padding: 10px; border-radius:10px; background: rgba(240, 240, 240, 1); display: block; }");
                stream.WriteLine("</style>");
                stream.WriteLine("</head>");
                stream.WriteLine("<body>");

                renderSection("From", $"{EmailFound.From.DisplayName} ({EmailFound.From.Address})");
                for (var i = 0; i < EmailFound.To.Count; i++)
                {
                    renderSection($"To[{i}]", $"{EmailFound.To.ElementAt(i).DisplayName} ({EmailFound.To.ElementAt(i).Address})");
                }
                renderSection("Subject", EmailFound.Subject);
                renderSection("Body", EmailFound.Body);

                stream.WriteLine("</body>");


                stream.WriteLine("</html>");
            }
            this.fileSaverService.Save(file);



            return EmailFound;
        }
        public void ClearEmailBox(UserType user)
        {
            Assert.IsTrue(this.emailService.DeleteAllMessages(user.Email, user.Password).Ok);
        }
    }

    

    public static class EmailDependenciesExtensions
    {
        public static IEmailDependencies Email(this IDependencies dependency)
        {
            return dependency.Resolver().Resolve<IEmailDependencies>();
        }
    }
}
