using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Traps.Queries
{
    public class TrapDto : IMapFrom<Trap>
    {
        public Guid TrapId { get; set; }
        public string TrapName { get; set; }
        public string Color { get; set; }
        public ICollection<FieldDto> Fields { get; init; } = new List<FieldDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Trap, TrapDto>()
              .ForMember(dest => dest.TrapId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
