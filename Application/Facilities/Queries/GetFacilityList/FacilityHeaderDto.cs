using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Facilities.Queries.GetFacilityList {
  public class FacilityHeaderDto : IMapFrom<Facility> {
    public Guid FacilityId { get; init; }
    public string Name { get; init; }
    public string City { get; init; }
    public string Address { get; init; }

    public void Mapping(Profile profile) {
      profile.CreateMap<Facility, FacilityHeaderDto>().ForMember(src => src.FacilityId, opt => opt.MapFrom(src => src.Id));
    }
  }
}
