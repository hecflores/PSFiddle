using AutoMapper;
using System;
using MC.Track.TestSuite.Model.Dto;
using MC.Track.TestSuite.Model.Types;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.UI.Types;

namespace MC.Track.TestSuite.Model.AutoMappers
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<OrganizationDto, OrganizationType>()
                .ForMember(d => d.Address1, o => o.Ignore())
                .ForMember(d => d.AlphaID, o => o.Ignore())
                .ForMember(d => d.City, o => o.Ignore())
                .ForMember(d => d.Country, o => o.Ignore())
                .ForMember(d => d.RegisteredBusinessName, o => o.Ignore())
                .ForMember(d => d.State, o => o.Ignore())
                .ForMember(d => d.Zip, o => o.Ignore());


            //CreateMap<FinancialInstitutionOrganizationType, FinancialInstitutionOrganizationDto>()
            //    .ForMember(d => d.FinancialInstitutionKey, o => o.Ignore())
            //    .ForMember(d => d.CardOnFile, o => o.Ignore())
            //    .ForMember(d => d.CoFCVC2, o => o.Ignore())
            //    .ForMember(d => d.PaymentPreferences, o => o.Ignore());

        }
    }
}
