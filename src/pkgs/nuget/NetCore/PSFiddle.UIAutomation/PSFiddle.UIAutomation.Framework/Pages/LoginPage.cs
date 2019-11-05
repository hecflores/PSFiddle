using MC.Track.TestSuite.Model.Types;
using MC.Track.TestSuite.Toolkit.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Toolkit.Extensions;
using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Pages;
using MC.Track.TestSuite.Interfaces.Pages.Shared;


using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Model.Enums;


namespace MC.Track.TestSuite.Toolkit.Pages
{
    [AthenaRegister(typeof(ILoginPage), AthenaRegistrationType.Singleton)]
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public class LoginPage : RawPage, ILoginPage
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        
        //public IHomePage LoginAs(UserType user)
        //{
        //    var config = resolver.Resolve<IConfiguration>();
        //    this.Browser.TrackLogin(resolver, user.Email, user.Password, config.HostUrl);

        //    return TransitionPage<HomePage>();
        //}
        //public IUserAgreementPage LoginAsExpectTermsAndConditions(UserType user)
        //{
        //    var config = resolver.Resolve<IConfiguration>();
        //    this.Browser.TrackLogin(resolver, user.Email, user.Password, config.HostUrl);

        //    return TransitionPage<UserAgreementPage>();
        //}

        //public IHomePage AnonymousUserLaunch()
        //{
        //    return TransitionPage<HomePage>();
        //}

        public override void ValidatePage()
        {
            // No Validation Yet
        }
    }
}
