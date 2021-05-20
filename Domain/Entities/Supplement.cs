using Domain.Common;
using System;
using System.IO;

namespace Domain.Entities {
  public class Supplement : AuditableEntity {
    public string SupplementName { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string CertificateFilePath { get; private set; }

    public Guid ProviderId { get; set; }
    public IUser Provider { get; set; }

    public void GeneratePath(string fileFormat) {
      CertificateFilePath = Path.Combine("supplements", "certificate", $"{Id}.{fileFormat}");
    }
  }
}
