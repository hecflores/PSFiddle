using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AE.Net.Mail;

namespace MC.Track.TestSuite.Toolkit.Extensions
{
    public static class MailMessageService
    {
        public static string ExtractJoinTrackUrl(this IMailMessageService mailMessageService, MailMessage mailMessage)
        {
            var links = mailMessageService.GetLink(mailMessage);

            return links;


        }
    }
}
