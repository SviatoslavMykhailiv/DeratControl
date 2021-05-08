using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Perimeters.Queries.GetPerimeterDetail {
  public class PerimeterVm : IMapFrom<Perimeter> {
    public Guid PerimeterId { get; set; }
    public string PerimeterName { get; set; }
    public string SchemeImagePath { get; set; }
    public int TopLoc { get; set; }
    public int LeftLoc { get; set; }
    public ICollection<PointDto> Points { get; set; }

    public void Mapping(Profile profile) {
      profile.CreateMap<Perimeter, PerimeterVm>()
        .ForMember(dest => dest.PerimeterId, opt => opt.MapFrom(src => src.Id));
    }
  }
}
