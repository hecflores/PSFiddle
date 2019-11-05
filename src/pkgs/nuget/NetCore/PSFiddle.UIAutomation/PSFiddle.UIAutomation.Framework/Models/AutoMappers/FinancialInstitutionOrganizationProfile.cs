
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MC.Track.TestSuite.Model.Dto;
using MC.Track.TestSuite.Model.Types;
namespace MC.Track.TestSuite.Model.AutoMappers
{
    public class FinancialInstitutionOrganizationProfile : Profile
    {
        public FinancialInstitutionOrganizationProfile()
        {
            CreateMap<FinancialInstitutionOrganizationDto, FinancialInstitutionOrganizationType>()
                .ForMember(d => d.FinInstOrgKey, o => o.Ignore())
                .ForMember(d => d.FinancialInstitutionID, o => o.Ignore())
                .ForMember(d => d.AlphaID, o => o.Ignore())
                .ForMember(d => d.MerchantID, o => o.Ignore())
                //Need this parameter for user story 20450
                //.ForMember(d => d.CardScheme, o => o.Ignore())
                .ForMember(d => d.Status, o => o.Ignore())
                .ForMember(d => d.StatusKey, o => o.Ignore())
                .ForMember(d => d.ComplianceDataDefault, o => o.Ignore())
                .ForMember(d => d.PaymentServicesDefault, o => o.Ignore())
                .ForMember(d => d.PremiumMonitoringDefault, o => o.Ignore())
                .ForMember(d => d.BillingDefault, o => o.Ignore());


            CreateMap<FinancialInstitutionOrganizationType, FinancialInstitutionOrganizationDto>()
                .ForMember(d => d.FinancialInstitutionKey, o => o.Ignore())
                .ForMember(d => d.CardOnFile, o => o.Ignore())
                .ForMember(d => d.CoFCVC2, o => o.Ignore())
                .ForMember(d => d.PaymentPreferences, o => o.Ignore());

        }
    }

}
