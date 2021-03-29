using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities {
  public class PointReview : AuditableEntity {
    public PointReview() {
      Records = new HashSet<PointReviewRecord>();
    }

    public Guid PointReviewId { get; set; }
    public Guid ErrandId { get; set; }
    public Guid PointId { get; set; }

    public PointReviewStatus Status { get; set; }
    public Errand Errand { get; set; }
    public Point Point { get; set; }
    public ICollection<PointReviewRecord> Records { get; private set; }
  }
}
