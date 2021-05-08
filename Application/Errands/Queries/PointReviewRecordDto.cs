using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Errands.Queries {
  public class PointReviewRecordDto : IMapFrom<PointReviewRecord> {
    public string FieldName { get; init; }
    public string Value { get; init; }
    public FieldType FieldType { get; init; }
    public string OptionList { get; init; }

    public void Mapping(Profile profile) {
      profile.CreateMap<PointReviewRecord, PointReviewRecordDto>()
        .ForMember(dest => dest.FieldType, opt => opt.MapFrom(src => src.Field.FieldType))
        .ForMember(dest => dest.OptionList, opt => opt.MapFrom(src => src.Field.OptionList))
        .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
        .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => src.Field.FieldName));
    }
  }
}
