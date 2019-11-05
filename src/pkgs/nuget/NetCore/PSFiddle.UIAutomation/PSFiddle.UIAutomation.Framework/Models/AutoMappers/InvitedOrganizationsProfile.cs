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
    public class InvitedOrganizationsProfile : Profile
    {
        public InvitedOrganizationsProfile()
        {
            CreateMap<InviteDto, InvitedOrganizationsType>()
                .ForMember(dest => dest.Organizationkey, opt => opt.MapFrom(src => src.InvitedOrganizationkey));
        }
    }
}
