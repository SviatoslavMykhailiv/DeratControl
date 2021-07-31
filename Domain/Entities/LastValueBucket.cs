using Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class LastValueBucket
    {
        private readonly Dictionary<PointFieldKey, string> values = new Dictionary<PointFieldKey, string>();

        public LastValueBucket()
        {

        }

        public LastValueBucket(IEnumerable<PointFieldValue> values)
        {
            foreach (var value in values)
            {
                var key = new PointFieldKey { PointId = value.PointId, FieldId = value.FieldId };
                this.values[key] = value.Value;
            }
        }

        public string this[Guid pointId, Guid fieldId]
        {
            get
            {
                var key = new PointFieldKey { PointId = pointId, FieldId = fieldId };
                return values.ContainsKey(key) ? values[key] : string.Empty;
            }
        }
    }
}
