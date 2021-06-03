using Domain.Common;
using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class PointReviewRecord : AuditableEntity
    {
        private string value;

        public Guid PointReviewId { get; init; }
        public Guid FieldId { get; init; }

        public string Value
        {
            get => value;

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                if (Field.FieldType == FieldType.Numeric && int.TryParse(value, out _) == false)
                    throw new InvalidOperationException("Value must be a number.");

                if (Field.IsOptionType)
                {
                    if (Field.ContainsOption(value) == false)
                        throw new InvalidOperationException("Value does't exist in option list.");
                }

                this.value = value;
            }
        }

        public CompletedPointReview PointReview { get; init; }
        public Field Field { get; init; }

        public string GetValue() 
        {
            if(Field.FieldType == FieldType.Boolean) 
            {
                return Convert.ToBoolean(Convert.ToInt16(Value)).ToString();
            }

            return value;
        }
    }
}
