using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using System;

namespace Application.Traps
{
    public class FieldDto : IMapFrom<Field>
    {
        public Guid? FieldId { get; init; }
        public string FieldName { get; init; }
        public FieldType FieldType { get; init; }
        public Option[] OptionList { get; init; }
        public int Order { get; init; }
        public int PercentStep { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Field, FieldDto>()
              .ForMember(dest => dest.FieldId, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.OptionList, opt => opt.MapFrom(src => src.OptionList ?? Array.Empty<Option>()));
        }
    }
}
