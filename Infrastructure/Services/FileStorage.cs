using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services {
  public class FileStorage : IFileStorage {
    private const string BASE_FILE_PATH_NAME = "DeratFolder";

    private readonly IConfiguration configuration;

    public FileStorage(IConfiguration configuration) {
      this.configuration = configuration;
    }

    public Task<byte[]> ReadFile(string path) {
      return File.ReadAllBytesAsync(Path.Combine(configuration[BASE_FILE_PATH_NAME], path));
    }

    public Task Remove(string path) {
      var fullPath = Path.Combine(configuration[BASE_FILE_PATH_NAME], path);

      if (File.Exists(fullPath))
        File.Delete(fullPath);

      return Task.CompletedTask;
    }

    public Task SaveFile(string path, byte[] file) {
      var sections = path.Split(Path.DirectorySeparatorChar);
      var fileName = sections[^1];

      var fullPath = Path.Combine(configuration[BASE_FILE_PATH_NAME], path.Replace(fileName, string.Empty));

      if (Directory.Exists(fullPath) == false)
        Directory.CreateDirectory(fullPath);

      return File.WriteAllBytesAsync(Path.Combine(fullPath, fileName), file);
    }
  }
}
