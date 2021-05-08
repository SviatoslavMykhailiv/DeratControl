using Application.Common.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Infrastructure.Services {
  public class AESEncryptionService : IEncryptionService {
    private readonly IOptions<EncryptionOptions> options;

    public AESEncryptionService(IOptions<EncryptionOptions> options) {
      this.options = options;

      if (options.Value.Key.Any() == false)
        throw new ArgumentNullException(nameof(options.Value.Key));

      if (options.Value.IV.Any() == false)
        throw new ArgumentNullException(nameof(options.Value.IV));
    }

    public string Encrypt(string plainText) {
      if (string.IsNullOrWhiteSpace(plainText))
        throw new ArgumentNullException(nameof(plainText));

      using var crypter = Aes.Create();
      crypter.Key = options.Value.Key;
      crypter.IV = options.Value.IV;

      var encryptor = crypter.CreateEncryptor(options.Value.Key, options.Value.IV);

      using var msEncrypt = new MemoryStream();
      using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
      using (var swEncrypt = new StreamWriter(csEncrypt)) {
        swEncrypt.Write(plainText);
      }

      return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public string Decrypt(string encryptedValue) {
      if (string.IsNullOrWhiteSpace(encryptedValue))
        throw new ArgumentNullException(nameof(encryptedValue));

      var valueBytes = Convert.FromBase64String(encryptedValue);

      using var crypter = Aes.Create();
      crypter.Key = options.Value.Key;
      crypter.IV = options.Value.IV;

      var decryptor = crypter.CreateDecryptor(options.Value.Key, options.Value.IV);

      using var msEncrypt = new MemoryStream(valueBytes);
      using var csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read);
      using var swDecrypt = new StreamReader(csEncrypt);

      return swDecrypt.ReadToEnd();
    }
  }
}
