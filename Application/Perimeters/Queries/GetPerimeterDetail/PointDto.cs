using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Perimeters.Queries.GetPerimeterDetail {
  public class PointDto : IMapFrom<Point> {
    public Guid PointId { get; set; }
    public int Order { get; set; }
    public int LeftLoc { get; set; }
    public int TopLoc { get; set; }
    public Guid SupplementId { get; set; }
    public Guid TrapId { get; set; }

    public void Mapping(Profile profile) {
      profile.CreateMap<Point, PointDto>()
        .ForMember(dest => dest.PointId, opt => opt.MapFrom(src => src.Id));
    }
  }
}
