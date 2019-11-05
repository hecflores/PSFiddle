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
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<SubscriptionRequestDto, SubscriptionRequestType>();
            CreateMap<SubscriptionRequestType, SubscriptionRequestDto>();
            CreateMap<SubscriptionRequestType, SubscriptionStatusReturnDto>();
        }
    }
}
