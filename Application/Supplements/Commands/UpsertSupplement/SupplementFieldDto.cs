using Domain.Enums;
using System;

namespace Application.Supplements.Commands.UpsertSupplement {
  public class SupplementFieldDto {
    public Guid? SupplementFieldId { get; set; }
    public string FieldName { get; set; }
    public SupplementFieldType FieldType { get; set; }
  }
}
