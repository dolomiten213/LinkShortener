
using LinkShortener.Interfaces;
using QRCoder;

namespace LinkShortener.Services;

public class QrService : IQrService
{
    public Stream MakeQr(string content)
    {
        // var myBarcode = BarcodeWriter.CreateBarcode(content, BarcodeWriterEncoding.EAN8);
        // return myBarcode.BinaryStream;
        
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new BitmapByteQRCode(qrCodeData);
        var qrCodeAsBitmapByteArr = qrCode.GetGraphic(20);
        return new MemoryStream(qrCodeAsBitmapByteArr);
    }
}