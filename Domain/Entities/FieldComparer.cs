using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class FieldComparer : IEqualityComparer<Field>
    {
        public bool Equals(Field x, Field y)
        {
            return string.Equals(x.FieldName, y.FieldName, StringComparison.OrdinalIgnoreCase) && x.Order.Equals(y.Order);
        }

        public int GetHashCode([DisallowNull] Field obj)
        {
            return obj.FieldName.GetHashCode() ^ obj.Order.GetHashCode() ^ 31;
        }
    }
}
