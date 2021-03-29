using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Traps.Queries.GetTrapList {
  public class TrapDto : IMapFrom<Trap> {
    public Guid TrapId { get; set; }
    public string TrapName { get; set; }
    public string Color { get; set; }

    public void Mapping(Profile profile) {
      profile.CreateMap(typeof(Trap), GetType());
    }
  }
}
