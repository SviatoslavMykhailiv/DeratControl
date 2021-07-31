using Domain.Common;
using Domain.ValueObjects;
using Domain.ValueObjects.FieldTypes;
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
          int percentStep,
          bool adminEditable)
        {

            if (fieldId is null)
                AddField(fieldName, order, type, optionList, percentStep, adminEditable);
            else
                UpdateField(fieldId.Value, fieldName, order, type, optionList, percentStep, adminEditable);
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
          int percentStep,
          bool adminEditable)
        {
            var field = new Field
            {
                FieldName = fieldName,
                Order = order,
                FieldType = type,
                OptionList = optionList,
                PercentStep = percentStep,
                AdminEditable = adminEditable
            };

            fields.Add(field);
        }

        private void UpdateField(
          Guid fieldId,
          string fieldName,
          int order,
          FieldType type,
          Option[] optionList,
          int percentStep,
          bool adminEditable)
        {
            var field = fields.First(f => f.Id == fieldId);

            field.FieldName = fieldName;
            field.Order = order;
            field.FieldType = type;
            field.OptionList = optionList;
            field.PercentStep = percentStep;
            field.AdminEditable = adminEditable;
        }

        public void AssertFieldsUnique()
        {
            var unique = fields.Distinct(new FieldComparer()).Count().Equals(fields.Count);

            if (unique == false)
                throw new InvalidOperationException("Усі поля мають бути унікальні за назвою та порядковим номером.");
        }
    }
}
