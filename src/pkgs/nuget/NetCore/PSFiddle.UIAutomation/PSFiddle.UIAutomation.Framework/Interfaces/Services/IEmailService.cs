using MC.Track.TestSuite.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AE.Net.Mail;
using Jane;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IEmailService
    {
        IResult<List<MailMessage>> ReceiveMails(String UserName, String Password);
        IResult DeleteAllMessages(String UserName, String Password);
    }
}
