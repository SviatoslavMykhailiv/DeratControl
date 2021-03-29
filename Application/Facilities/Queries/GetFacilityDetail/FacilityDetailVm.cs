using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Facilities.Queries.GetFacilityDetail {
  public class FacilityDetailVm : IMapFrom<Facility> {
    public Guid FacilityId { get; set; }
    public string CompanyName { get; set; }
    public string Name { get; set; }
    public string SecurityCode { get; set; }
    public string Address { get; set; }
    public ICollection<PerimeterHeaderDto> Perimeters { get; set; }
    public ICollection<UserHeaderDto> Users { get; set; }

    public void Mapping(Profile profile) {
      profile.CreateMap<Facility, FacilityDetailVm>();
    }
  }
}
