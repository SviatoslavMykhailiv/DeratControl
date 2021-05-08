﻿using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Users.Queries {
  public record UserDto : IMapFrom<IUser> {
    public Guid UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PhoneNumber { get; init; }
    public string Location { get; init; }
    public ICollection<Guid> DefaultFacilities { get; init; } = new List<Guid>();

    public void Mapping(Profile profile) {
      profile.CreateMap<IUser, UserDto>()
       .ForMember(dest => dest.DefaultFacilities, opt => opt.MapFrom(src => src.DefaultFacilities.Select(d => d.FacilityId).ToList()));
    }
  }
}
