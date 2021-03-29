using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Facilities.Queries.GetFacilityDetail {
  public class PerimeterHeaderDto : IMapFrom<Perimeter> {
    public Guid PerimeterId { get; set; }
    public string PerimeterName { get; set; }

    public void Mapping(Profile profile) {
      profile.CreateMap<Perimeter, PerimeterHeaderDto>();
    }
  }
}
