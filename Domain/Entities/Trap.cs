using Domain.Common;
using System;

namespace Domain.Entities {
  public class Trap : AuditableEntity {
    public Guid TrapId { get; set; }
    public string TrapName { get; set; }
    public string Color { get; set; }
  }
}
