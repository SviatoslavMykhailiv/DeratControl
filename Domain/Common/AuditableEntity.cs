using System;

namespace Domain.Common {
  public abstract class AuditableEntity {
    public DateTime ModifiedAt { get; set; }
    public Guid ModifiedBy { get; set; }
  }
}
