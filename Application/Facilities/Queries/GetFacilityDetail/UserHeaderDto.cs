using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Facilities.Queries.GetFacilityDetail {
  public class UserHeaderDto : IMapFrom<IUser> {
    public Guid UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public void Mapping(Profile profile) {
      profile.CreateMap<IUser, UserHeaderDto>();
    }
  }
}
