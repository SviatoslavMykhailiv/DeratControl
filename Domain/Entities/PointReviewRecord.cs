using Domain.Common;
using System;

namespace Domain.Entities {
  public class PointReviewRecord : AuditableEntity {
    public Guid PointReviewRecordId { get; set; }
    public Guid PointReviewId { get; set; }
    public Guid SupplementFieldId { get; set; }
    public string Value { get; set; }
    public PointReview PointReview { get; set; }
    public SupplementField SupplementField { get; set; }
  }
}
