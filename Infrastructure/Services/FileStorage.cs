using Application.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services {
  public class FileStorage : IFileStorage {
    public Task SaveFile(string path, byte[] file) {
      return Task.CompletedTask;
    }
  }
}
