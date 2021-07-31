using Domain.Common;
using System;

namespace Domain.Entities
{
    public class PointFieldValue : AuditableEntity
    {
        public Guid FieldId { get; init; }
        public Guid PointId { get; init; }
        public Field Field { get; init; }
        public Point Point { get; init; }
        public string Value { get; set; }
    }
}
