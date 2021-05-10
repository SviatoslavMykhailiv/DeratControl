using System;

namespace Domain.ValueObjects {
  public class Image {

    private readonly byte[] byteArray;
    public string Format { get; }

    public static implicit operator Image(string input) {
      if (string.IsNullOrEmpty(input))
        return null;

      var tokenList = input.Split(',');

      if (tokenList.Length == 1)
        return new Image(Convert.FromBase64String(tokenList[0]), null);

      var format = tokenList[0].Replace("data:image/", string.Empty).Replace(";base64", string.Empty);

      return new Image(Convert.FromBase64String(tokenList[1]), format);
    }

    private Image(byte[] byteArray, string format) {
      this.byteArray = byteArray;
      Format = format;
    }

    public static implicit operator byte[] (Image str) => str?.byteArray;
  }
}
