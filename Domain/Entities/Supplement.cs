using Domain.Common;
using System;

namespace Domain.Entities {
  public class Supplement : AuditableEntity {
    public string SupplementName { get; set; }
    public DateTime ExpirationDate { get; set; }
  }
}
