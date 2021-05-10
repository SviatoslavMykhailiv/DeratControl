using System.Threading.Tasks;

namespace Application.Common.Interfaces {
  public interface IFileStorage {
    Task SaveFile(string path, byte[] file);
    Task<byte[]> ReadFile(string path);
    Task Remove(string path);
  }
}
