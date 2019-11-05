using AutoMapper;
using MC.Track.TestSuite.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.AutoMappers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ComplianceMatchProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComplianceMatchProfile"/> class.
        /// </summary>
        public ComplianceMatchProfile()
        {
            CreateMap<ComplianceMatchEntity, MatchesGetResponse>()
                .ForMember(d => d.ComplianceAlertsCount, o => o.MapFrom(src => src.NoofAlerts))
                .ForMember(d => d.HasComplianceRecord, o => o.MapFrom(src => src.HasValue))
                .ForMember(d => d.IsLinked, o => o.MapFrom(src => src.HasLinked))
                .ForMember(d => d.IsPurchased, o => o.MapFrom(src => src.HasBeenPurchased))
                .ForMember(d => d.LatestComplianceDate, o => o.MapFrom(src => src.LatestDate));
        }
    }
}
