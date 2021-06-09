using Application.Common.Interfaces;
using QRCoder;
using System.Drawing.Imaging;
using System.IO;

namespace Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {
        private readonly QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();

        public byte[] Generate(string value)
        {
            var qrCodeData = qRCodeGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            using var memoryStream = new MemoryStream();

            qrCodeImage.Save(memoryStream, ImageFormat.Png);
            return memoryStream.ToArray();
        }
    }
}
