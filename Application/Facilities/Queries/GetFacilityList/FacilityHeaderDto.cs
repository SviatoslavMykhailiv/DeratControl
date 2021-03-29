using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Facilities.Queries.GetFacilityList {
  public class FacilityHeaderDto : IMapFrom<Facility> {
    public Guid FacilityId { get; set; }
    public string Name { get; set; }

    public void Mapping(Profile profile) {
      profile.CreateMap<Facility, FacilityHeaderDto>();
    }
  }
}
