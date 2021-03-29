using Domain.Common;
using Domain.Enums;
using System;

namespace Domain.Entities {
  public class SupplementField : AuditableEntity {
    public Guid SupplementFieldId { get; set; }
    public Guid SupplementId { get; set; }
    public string FieldName { get; set; }
    public int Order { get; set; }
    public SupplementFieldType SupplementFieldType { get; set; }
    public Supplement Supplement { get; set; }
  }
}
