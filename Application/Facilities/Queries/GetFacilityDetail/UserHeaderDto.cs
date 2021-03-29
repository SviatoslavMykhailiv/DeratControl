using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Facilities.Queries.GetFacilityDetail {
  public class UserHeaderDto : IMapFrom<IUser> {
    public Guid UserId { get; set; }
    public string UserName { get; set; }

    public void Mapping(Profile profile) {
      profile.CreateMap<IUser, UserHeaderDto>();
    }
  }
}
