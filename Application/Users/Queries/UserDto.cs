using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Users.Queries
{
    public record UserDto : IMapFrom<IUser>
    {
        public Guid UserId { get; init; }
        public string UserName { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string PhoneNumber { get; init; }
        public string Location { get; init; }
        public bool Available { get; init; }
        public ICollection<Guid> Facilities { get; init; } = new List<Guid>();
        public Device Device { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IUser, UserDto>()
              .ForMember(dest => dest.Facilities, opt => opt.MapFrom(src => src.DefaultFacilities.Select(d => d.FacilityId).ToList()))
              .ForMember(dest => dest.Device, opt => opt.MapFrom(src => src.Device ?? new Device(string.Empty, Domain.Enums.DeviceType.iOS)));
        }
    }
}
