using System;

namespace Infrastructure.Options
{
    public class EncryptionOptions
    {
        public byte[] IV { get; set; } = Array.Empty<byte>();
        public byte[] Key { get; set; } = Array.Empty<byte>();
    }
}
