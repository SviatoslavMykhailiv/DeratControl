using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class CompletedPointReview : AuditableEntity
    {
        private readonly HashSet<PointReviewRecord> records = new HashSet<PointReviewRecord>();

        public CompletedPointReview()
        {

        }

        public CompletedPointReview(Point point, string report)
        {
            Report = report;
            PointId = point.Id;
            Perimeter = point.Perimeter;
            PointOrder = point.Order;
            Trap = point.Trap;
            Supplement = point.Supplement;
            PerimeterId = point.PerimeterId;
            TrapId = point.TrapId;
            SupplementId = point.SupplementId;
            Report = report;
        }

        public Guid ErrandId { get; init; }
        public CompletedErrand Errand { get; init; }
        public Guid PointId { get; init; }

        public Guid PerimeterId { get; init; }
        public Perimeter Perimeter { get; init; }

        public int PointOrder { get; init; }

        public Trap Trap { get; init; }
        public Guid TrapId { get; init; }

        public Supplement Supplement { get; init; }
        public Guid SupplementId { get; init; }

        public string Report { get; init; }

        public IEnumerable<PointReviewRecord> Records => records;

        public void AddRecord(Field field, string value)
        {
            var record = new PointReviewRecord { Field = field, Value = value };
            records.Add(record);
        }
    }
}
