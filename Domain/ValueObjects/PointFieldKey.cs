using System;

namespace Domain.ValueObjects
{
    public struct PointFieldKey
    {
        public Guid FieldId { get; init; }
        public Guid PointId { get; init; }
    }
}
