using System.Threading.Tasks;

namespace Application.Common.Interfaces {
  public interface IFileStorage {
    Task SaveFile(string path, byte[] file);
  }
}
