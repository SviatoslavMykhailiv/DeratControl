using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Facilities.Queries.GetFacilityDetail
{
    public class PerimeterHeaderDto : IMapFrom<Perimeter>
    {
        public Guid PerimeterId { get; init; }
        public string PerimeterName { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Perimeter, PerimeterHeaderDto>()
              .ForMember(dest => dest.PerimeterId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
