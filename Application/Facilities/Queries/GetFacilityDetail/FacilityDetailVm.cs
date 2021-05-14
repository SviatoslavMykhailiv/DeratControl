using Application.Common.Mappings;
using Application.Perimeters.Queries.GetPerimeterDetail;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Facilities.Queries.GetFacilityDetail
{
    public class FacilityDetailVm : IMapFrom<Facility>
    {
        public Guid FacilityId { get; init; }
        public string CompanyName { get; private set; }
        public string Name { get; init; }
        public string City { get; init; }
        public string SecurityCode { get; private set; }
        public string Address { get; init; }
        public ICollection<PerimeterVm> Perimeters { get; init; }
        public ICollection<UserHeaderDto> Users { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Facility, FacilityDetailVm>()
              .ForMember(dest => dest.FacilityId, opt => opt.MapFrom(src => src.Id));
        }

        public void Obfuscate()
        {
            CompanyName = Name;
            SecurityCode = string.Empty;
        }
    }
}
