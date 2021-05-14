using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Perimeters.Queries.GetPerimeterDetail
{
    public class PerimeterVm : IMapFrom<Perimeter>
    {
        public Guid PerimeterId { get; init; }
        public string PerimeterName { get; init; }
        public string SchemeImage { get; init; }
        public int TopLoc { get; init; }
        public int LeftLoc { get; init; }
        public decimal Scale { get; init; }
        public ICollection<PointDto> Points { get; init; } = new List<PointDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Perimeter, PerimeterVm>()
              .ForMember(dest => dest.SchemeImage, opt => opt.MapFrom(src => src.SchemeImagePath))
              .ForMember(dest => dest.PerimeterId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
