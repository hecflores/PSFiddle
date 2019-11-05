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
    public class CCProfile : Profile
    {
        public CCProfile()
        {
            CreateMap<FinancialInstitutionOrganizationType, CCReturnDto>()
                .ForMember(x => x.cardScheme, y => y.MapFrom(m => FormatCardScheme(m.CardScheme)))
                .ForMember(x => x.currency, y => y.MapFrom(m => m.Currency))
                .ForMember(x => x.isBillingDefault, y => y.MapFrom(m => m.BillingDefault))
                .ForMember(x => x.isComplianceDefault, y => y.MapFrom(m => m.ComplianceDataDefault))
                .ForMember(x => x.isPremiumMonitoringDefault, y => y.MapFrom(m => m.PremiumMonitoringDefault))
                .ForMember(x => x.isSubscriptionDefault, y => y.MapFrom(m => m.PaymentServicesDefault))
                .ForMember(x => x.id, y => y.MapFrom(m => m.FinInstOrgKey))
                .ForMember(x => x.status, y => y.MapFrom(m => m.Status))
                // Added maskednumber field for US20450
                .ForMember(x => x.maskedNumber, y => y.MapFrom(m => m.MaskedNumber)); ;
        }

        

        private static string FormatCardScheme(string input)
        {
            string allLower = input.ToLower();

            return input.First().ToString().ToUpper() + allLower.Substring(1);

        }

        private static string CreateStars (int number)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < number; i++){
                sb.Append("*");
            }

            return sb.ToString();
        }
    }
}
