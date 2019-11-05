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
    public class FinancialInstitutionProfile : Profile
    {
        public FinancialInstitutionProfile()
        {
            CreateMap<FinancialInstitutionDto, FinancialInstitutionType>()
                .ForMember(d => d.FinancialInstitutionKey, o => o.Ignore())
                .ForMember(d => d.AccountName, o => o.Ignore())
                .ForMember(d => d.PaymentType, o => o.Ignore())
                .ForMember(d => d.CardOnFile, o => o.Ignore())
                .ForMember(d => d.CoFCVC2, o => o.Ignore())
                .ForMember(d => d.CoFtoken, o => o.Ignore())
                .ForMember(d => d.AccountNumber, o => o.Ignore())
                .ForMember(d => d.PaymentPreferences, o => o.Ignore())
                .ForMember(d => d.Currency, o => o.Ignore());

            CreateMap<FinancialInstitutionType, FinancialInstitutionDto>()
                .ForMember(d => d.CreatedDate, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.CreatedByKey, o => o.Ignore())
                .ForMember(d => d.ModifiedDate, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore())
                .ForMember(d => d.ModifiedByKey, o => o.Ignore());






        }
    }
}
