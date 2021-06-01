using Domain.Common;
using Domain.Enums;
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

        public CompletedPointReview(Point point, PointReviewStatus status, string report)
        {
            Status = status;
            Report = report;
            PointId = point.Id;
            PerimeterName = point.Perimeter.PerimeterName;
            PointOrder = point.Order;
            Trap = point.Trap.TrapName;
            Supplement = point.Supplement.SupplementName;
        }

        public Guid ErrandId { get; init; }
        public CompletedErrand Errand { get; init; }
        public Guid PointId { get; init; }
        public string PerimeterName { get; init; }
        public int PointOrder { get; init; }
        public string Trap { get; init; }
        public string Supplement { get; init; }
        public PointReviewStatus Status { get; init; }
        public string Report { get; }

        public IEnumerable<PointReviewRecord> Records => records;

        public void AddRecord(Field field, string value)
        {
            var record = new PointReviewRecord { Field = field, Value = value };
            records.Add(record);
        }
    }
}
