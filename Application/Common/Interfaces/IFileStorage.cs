using System;
using System.Threading.Tasks;

namespace Application.Common.Interfaces {
  public interface IFileStorage {
    Task<string> SaveCertificate(Guid supplementId, byte[] certificate);
    Task SavePerimeterScheme(string path, byte[] scheme);
  }
}
