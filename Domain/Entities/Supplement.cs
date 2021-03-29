using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities {
  public class Supplement : AuditableEntity {
    public Supplement() {
      Fields = new HashSet<SupplementField>();
    }

    public Guid SupplementId { get; set; }
    public string SupplementName { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string CertificatePath { get; set; }

    public ICollection<SupplementField> Fields { get; private set; }
  }
}
