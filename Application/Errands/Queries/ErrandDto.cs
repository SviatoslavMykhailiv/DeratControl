using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Errands.Queries {
  public class ErrandDto : IMapFrom<Errand> {
    public Guid ErrandId { get; init; }
    public string Description { get; init; }
    public string FacilityName { get; init; }
    public string City { get; init; }
    public string Address { get; init; }
    public string DueDate { get; init; }

    public ICollection<PointReviewDto> Points { get; init; } = new List<PointReviewDto>();

    public void Mapping(Profile profile) {
      profile.CreateMap<Errand, ErrandDto>()
        .ForMember(dest => dest.ErrandId, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.FacilityName, opt => opt.MapFrom(src => src.Facility.Name))
        .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Facility.City))
        .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Facility.Address))
        .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.ToShortDateString()));
    }
  }
}
