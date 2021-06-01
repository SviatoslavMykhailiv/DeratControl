using System;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.Errands.Queries {
  public class PointReviewRecordDto {
    public Guid FieldId { get; init; }
    public string FieldName { get; init; }
    public FieldType FieldType { get; init; }
    public Option[] OptionList { get; init; }
  }
}
