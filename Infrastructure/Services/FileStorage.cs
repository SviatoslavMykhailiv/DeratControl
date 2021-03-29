using Application.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services {
  public class FileStorage : IFileStorage {
    public Task<string> SaveCertificate(Guid supplementId, byte[] certificate) {
      return Task.FromResult(string.Empty);
    }

    public async Task SavePerimeterScheme(string path, byte[] scheme) {
      
    }
  }
}
