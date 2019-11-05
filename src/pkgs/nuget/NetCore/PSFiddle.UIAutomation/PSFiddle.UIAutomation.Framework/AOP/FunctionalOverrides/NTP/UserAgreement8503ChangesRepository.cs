
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Repositories;
using MC.Track.TestSuite.Model.Types;
using MC.Track.TestSuite.Proxies.FunctionalOverrides.NTP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace MC.Track.TestSuite.Proxies.FunctionalOverrides.NTP
{
    // TODO DELETE
    //[AthenaInterceptor(typeof(IUserAgreementRepository))]
    //public class UserAgreement8503ChangesRepository : IInterceptionBehavior
    //{
    //    private readonly IBaseRepository<UserAgreementType> _repository;
    //    private readonly IBaseRepository<UserAgreementStatus8503ChangesType> _statusRepo;

    //    public UserAgreement8503ChangesRepository(IBaseRepository<UserAgreementType> repo, IBaseRepository<UserAgreementStatus8503ChangesType> statusRepo)
    //    {
    //        _repository = repo;
    //        _statusRepo = statusRepo;
    //    }

    //    public bool WillExecute => true;

    //    public IEnumerable<Type> GetRequiredInterfaces()
    //    {
    //        return Type.EmptyTypes;
    //    }

    //    public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
    //    {
    //        if (input.Target is IUserAgreementRepository)
    //        {
    //            if (input.MethodBase.Name == "UpdateUserDataPrivacy")
    //            {
    //                long alphaUserKey = (long)input.Arguments["alphaUserKey"];
    //                bool status = (bool)input.Arguments["status"];
    //                var userAgreement = new UserAgreementStatus8503ChangesType
    //                {
    //                    AlphaUserKey = alphaUserKey,
    //                    IsAccepted = status,
    //                    TypeOfAgreement = "PN",
    //                    ModifiedByKey = alphaUserKey,
    //                    IsMarketingProduct = true,
    //                    IsMarketingTrack = true
    //                };
    //                return input.CreateMethodReturn(
    //                     _statusRepo.InsertObjectAsync("usp_InsertUpdateAgreementsAcceptance", userAgreement, "@AgreementsAcceptanceType")
    //                );

    //            }

    //            if (input.MethodBase.Name == "UpdateUserTermsOfUse")
    //            {
    //                long alphaUserKey = (long)input.Arguments["alphaUserKey"];
    //                bool status = (bool)input.Arguments["status"];
    //                var userAgreement = new UserAgreementStatus8503ChangesType
    //                {
    //                    AlphaUserKey = alphaUserKey,
    //                    IsAccepted = status,
    //                    TypeOfAgreement = "TOU",
    //                    ModifiedByKey = alphaUserKey,
    //                    IsMarketingProduct = true,
    //                    IsMarketingTrack = true
    //                };

    //                return input.CreateMethodReturn(
    //                    _statusRepo.InsertObjectAsync("usp_InsertUpdateAgreementsAcceptance", userAgreement, "@AgreementsAcceptanceType")
    //                );
    //            }
    //        }

    //        return getNext().Invoke(input, getNext);

            
    //    }
    //}
}
