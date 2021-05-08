using Domain.Entities;
using System;

namespace Domain.Common {
  public abstract class AuditableEntity : Entity {
    public DateTime ModifiedAt { get; set; }
    public Guid ModifiedBy { get; set; }
  }
}
