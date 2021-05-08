using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities {
  public class Trap : AuditableEntity {
    private readonly HashSet<Field> fields = new HashSet<Field>();

    public string TrapName { get; set; }
    public string Color { get; set; }

    public IEnumerable<Field> Fields => fields;

    public override string ToString() {
      return TrapName;
    }

    public void SetField(
      Guid? fieldId,
      string fieldName,
      int order,
      FieldType type,
      string optionList) {

      if (fieldId is null)
        AddField(fieldName, order, type, optionList);
      else
        UpdateField(fieldId.Value, fieldName, order, type, optionList);
    }

    public void RemoveField(Guid fieldId) {
      var field = fields.FirstOrDefault(f => f.Id == fieldId);

      if (field is null)
        return;

      fields.Remove(field);
    }

    private void AddField(
      string fieldName,
      int order,
      FieldType type,
      string optionList) {

      if (fields.Any(f => string.Equals(f.FieldName, fieldName, StringComparison.OrdinalIgnoreCase)))
        throw new InvalidOperationException($"Field with name {fieldName} already exists.");

      if (fields.Any(f => f.Order == order))
        throw new InvalidOperationException($"Field with order {order} already exists.");

      var field = new Field {
        FieldName = fieldName,
        Order = order,
        FieldType = type,
        OptionList = optionList
      };

      fields.Add(field);
    }

    private void UpdateField(
      Guid fieldId,
      string fieldName,
      int order,
      FieldType type,
      string optionList) {

      if (fields.Any(f => string.Equals(f.FieldName, fieldName, StringComparison.OrdinalIgnoreCase) && f.Id != fieldId))
        throw new InvalidOperationException($"Field with name {fieldName} already exists.");

      if (fields.Any(f => f.Order == order && f.Id != fieldId))
        throw new InvalidOperationException($"Field with order {order} already exists.");

      var field = fields.First(f => f.Id == fieldId);

      field.FieldName = fieldName;
      field.Order = order;
      field.FieldType = type;
      field.OptionList = optionList;
    }
  }
}
