using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class LastValueBucket
    {
        private readonly Dictionary<Guid, Dictionary<Guid, string>> values = new Dictionary<Guid, Dictionary<Guid, string>>();

        public LastValueBucket()
        {

        }

        public LastValueBucket(IEnumerable<CompletedPointReview> reviews)
        {
            foreach (var group in reviews.GroupBy(c => c.PointId))
            {
                var latest = group.OrderByDescending(c => c.ModifiedAt).FirstOrDefault();

                if (latest is null) continue;

                var recordDic = new Dictionary<Guid, string>();

                foreach (var record in latest.Records)
                {
                    recordDic.Add(record.FieldId, record.Value);
                }

                values.Add(group.Key, recordDic);
            }
        }

        public string this[Guid pointId, Guid fieldId]
        {
            get
            {
                if(values.ContainsKey(pointId) == false)
                {
                    return string.Empty;
                }

                var records = values[pointId];

                if(records.Count == 0)
                {
                    return string.Empty;
                }

                if(records.ContainsKey(fieldId) == false)
                {
                    return string.Empty;
                }

                return records[fieldId];
            }
        }
    }
}
