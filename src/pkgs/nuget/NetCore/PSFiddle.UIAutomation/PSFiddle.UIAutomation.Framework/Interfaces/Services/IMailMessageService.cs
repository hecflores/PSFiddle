using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Model.Domain;
using AE.Net.Mail;
namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IMailMessageService
    {
        bool VerifyNotificationEmail(MailMessage mailMessages, Dictionary<string, string> verifyValues);
        string GetLink(MailMessage message);
    }
}
