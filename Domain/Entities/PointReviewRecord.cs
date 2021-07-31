using Domain.Common;
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
                this.value = Field.FieldType.AdjustValue(Field, value);
            }
        }

        public CompletedPointReview PointReview { get; init; }
        public Field Field { get; init; }

        public string GetValue() 
        {
            return Field.FieldType.ToPrintFormat(Field, value);
        }
    }
}
