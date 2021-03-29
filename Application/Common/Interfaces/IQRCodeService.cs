namespace Application.Common.Interfaces {
  public interface IQRCodeService {
    byte[] Generate(string value);
  }
}
