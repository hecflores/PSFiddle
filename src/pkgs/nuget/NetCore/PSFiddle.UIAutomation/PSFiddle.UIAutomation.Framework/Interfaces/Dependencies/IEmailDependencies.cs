using AE.Net.Mail;
using MC.Track.TestSuite.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Dependencies
{
    public interface IEmailDependencies : IDependency
    {
        String GetFirstLink(MailMessage mailMessage);
        MailMessage VerifySentEmail(UserType user);
        void ClearEmailBox(UserType user);
    }
}
