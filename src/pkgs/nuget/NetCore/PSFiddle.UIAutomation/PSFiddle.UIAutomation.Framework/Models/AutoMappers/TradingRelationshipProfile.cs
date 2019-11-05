using AutoMapper;
using MC.Track.TestSuite.Model.Dto;
using MC.Track.TestSuite.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.AutoMappers
{
    public class TradingRelationshipProfile : Profile
    {
        public TradingRelationshipProfile()
        {
            CreateMap<TradingRelationshipType, TradingRelationshipGetDto>();
            CreateMap<TradingRelationshipPostDto, TradingRelationshipType>()
                .ForMember(x => x.CreatedByKey, y => y.MapFrom(z => z.UserKey))
                .ForMember(x => x.ModifiedByKey, y => y.MapFrom(z => z.UserKey))
                .ForMember(x => x.RemitToID, y => y.MapFrom(z => z.RemitToId))
                .ForMember(x => x.RemitFromID, y => y.MapFrom(z => z.RemitFromId))
                .ForMember(x => x.CreatedBy, y => y.Ignore())
                .ForMember(x => x.CreatedByKey, y => y.MapFrom(z => z.UserKey))
                .ForMember(x => x.CreatedDate, y => y.Ignore())
                .ForMember(x => x.ModifiedBy, y => y.Ignore())
                .ForMember(x => x.ModifiedByKey, y => y.MapFrom(z => z.UserKey))
                .ForMember(x => x.ModifiedDate, y => y.Ignore())
                .ForMember(x => x.PrimaryAlphaID, y => y.Ignore())
                .ForMember(x => x.SecondaryAlphaID, y => y.Ignore())
                .ForMember(x => x.PrimaryOrganizationType, y => y.Ignore())
                .ForMember(x => x.SecondaryOrganizationType, y => y.Ignore())
                .ForMember(x => x.PrimaryOrganizationOnboarded, y => y.Ignore())
                .ForMember(x => x.SecondaryOrganizationOnboarded, y => y.Ignore())
                .ForMember(x => x.TradingDescription, y => y.Ignore())
                .ForMember(x => x.TradingRelationship, y => y.Ignore())
                .ForMember(x => x.TradingRelationshipKey, y => y.Ignore());

            CreateMap<BuyerDto, BuyerType>().ReverseMap();

            CreateMap<SupplierDto, SupplierType>().ReverseMap();

        }
    }
}
