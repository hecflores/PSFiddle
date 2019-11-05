using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Model.Dto;
using MC.Track.TestSuite.Model.Types;
namespace MC.Track.TestSuite.Model.AutoMappers
{
    public class AcquiringBankProfile : Profile
    {
        public AcquiringBankProfile()
        {
            CreateMap<FinancialInstitutionOrganizationType, AcquiringBankReturnDto>()
                .ForMember(x => x.BankID, y => y.MapFrom(m => m.FinancialInstitutionID))
                .ForMember(x => x.BankName, y => y.MapFrom(m => m.FinancialInstitutionName))
                .ForMember(x => x.MerchantID, y => y.MapFrom(m => m.MerchantID))
                .ForMember(x => x.ID, y => y.MapFrom(m => m.FinInstOrgKey))
                .ForMember(x => x.BankMerchantID, y => y.MapFrom(m => m.AccountNumber))
                .ForMember(x => x.Currency, y => y.MapFrom(m => m.Currency))
                .ForMember(x => x.Status, y => y.MapFrom(m => m.Status))
                .ForMember(x => x.MerchantIDDefault, y => y.MapFrom(m => m.MerchantIDDefault));
        }
    }
}
