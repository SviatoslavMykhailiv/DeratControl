using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Trap : AuditableEntity
    {
        private readonly HashSet<Field> fields = new HashSet<Field>();

        public string TrapName { get; set; }
        public string Color { get; set; }

        public Guid ProviderId { get; set; }
        public IUser Provider { get; set; }

        public IEnumerable<Field> Fields => fields;

        public override string ToString()
        {
            return TrapName;
        }

        public void SetField(
          Guid? fieldId,
          string fieldName,
          int order,
          FieldType type,
          Option[] optionList,
          int percentStep)
        {

            if (fieldId is null)
                AddField(fieldName, order, type, optionList, percentStep);
            else
                UpdateField(fieldId.Value, fieldName, order, type, optionList, percentStep);
        }

        public void RemoveField(Guid fieldId)
        {
            var field = fields.FirstOrDefault(f => f.Id == fieldId);

            if (field is null)
                return;

            fields.Remove(field);
        }

        private void AddField(
          string fieldName,
          int order,
          FieldType type,
          Option[] optionList,
          int percentStep)
        {
            var field = new Field
            {
                FieldName = fieldName,
                Order = order,
                FieldType = type,
                OptionList = optionList,
                PercentStep = percentStep
            };

            fields.Add(field);
        }

        private void UpdateField(
          Guid fieldId,
          string fieldName,
          int order,
          FieldType type,
          Option[] optionList,
          int percentStep)
        {
            var field = fields.First(f => f.Id == fieldId);

            field.FieldName = fieldName;
            field.Order = order;
            field.FieldType = type;
            field.OptionList = optionList;
            field.PercentStep = percentStep;
        }

        public void AssertFieldsUnique()
        {
            var unique = fields.Distinct(new FieldComparer()).Count().Equals(fields.Count);

            if (unique == false)
                throw new InvalidOperationException("Усі поля мають бути унікальні за назвою та порядковим номером.");
        }
    }
}
