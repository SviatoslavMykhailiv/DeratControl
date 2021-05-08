using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities {
  public class PointReview : AuditableEntity {

    private readonly HashSet<PointReviewRecord> records = new HashSet<PointReviewRecord>();

    public PointReview() { }

    public PointReview(Point point) {
      Point = point;
      Status = PointReviewStatus.NotReviewed;

      AddFields();
    }

    public Guid ErrandId { get; init; }
    public Guid PointId { get; init; }
    public PointReviewStatus Status { get; private set; }
    public string Report { get; private set; }
    public Errand Errand { get; init; }
    public Point Point { get; private set; }
    public IEnumerable<PointReviewRecord> Records => records;

    public void Complete(PointReviewStatus status, string report, Dictionary<string, string> valueList) {
      if (status == PointReviewStatus.NotReviewed && string.IsNullOrWhiteSpace(report))
        throw new InvalidOperationException("Report cannot be empty.");

      Status = status;
      Report = report;

      if (Status == PointReviewStatus.Missing) {
        Errand.Facility.RemovePoint(PointId);
        Point = null;
      }

      foreach (var record in records)
        record.Value = valueList.GetValueOrDefault(record.Field.FieldName) ?? string.Empty;
    }

    private void AddFields() {
      foreach (var field in Point.Trap.Fields)
        records.Add(new PointReviewRecord { Field = field, Value = string.Empty });
    }

    public string this[Guid fieldId] => records.FirstOrDefault(v => v.Field.Id == fieldId)?.Value;
  }
}
