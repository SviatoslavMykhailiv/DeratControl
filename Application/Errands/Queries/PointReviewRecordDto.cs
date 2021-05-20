using System;
using Domain.Enums;

namespace Application.Errands.Queries {
  public class PointReviewRecordDto {
    public Guid RecordId { get; init; }
    public string FieldName { get; init; }
    public string Value { get; init; }
    public FieldType FieldType { get; init; }
    public string OptionList { get; init; }
  }
}
