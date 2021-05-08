using Domain.Common;
using Domain.Enums;
using System;

namespace Domain.Entities {
  public class Field : AuditableEntity {

    private string optionList;
    private FieldType type;

    public Guid TrapId { get; init; }
    public string FieldName { get; set; }
    public int Order { get; set; }

    public FieldType FieldType {
      get => type;
      set {
        if (value != FieldType.Option)
          optionList = string.Empty;

        type = value;
      }
    }

    public Trap Trap { get; init; }
    
    public string OptionList {
      get => optionList;
      set {
        if (type == FieldType.Option)
          optionList = value;
      }
    }
  }
}
