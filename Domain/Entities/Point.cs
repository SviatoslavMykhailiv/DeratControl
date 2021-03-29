using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities {
  public class Point : AuditableEntity {
    public Point() {
      Reviews = new HashSet<PointReview>();
    }

    public Guid PointId { get; set; }
    public Guid PerimeterId { get; set; }
    public Guid TrapId { get; set; }
    public Guid SupplementId { get; set; }

    public int Order { get; set; }
    public int LeftLoc { get; set; }
    public int TopLoc { get; set; }

    public Trap Trap { get; set; }
    public Supplement Supplement { get; set; }
    public Perimeter Perimeter { get; set; }

    public ICollection<PointReview> Reviews { get; private set; }
  }
}
