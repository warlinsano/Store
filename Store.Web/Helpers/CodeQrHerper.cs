using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZXing;
using ZXing.QrCode;


namespace Store.Web.Helpers
{
    public static class CodeQrHerper
    {
        //libreria QrCodeNet
        public static string generalCodeQr(string Value)
        {
            var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            var qrCode = new QrCode();
            qrEncoder.TryEncode(Value, out qrCode);

            GraphicsRenderer renderer = new GraphicsRenderer(new FixedCodeSize(400, QuietZoneModules.Zero), Brushes.Black, Brushes.White);
            MemoryStream ms = new MemoryStream();
            renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
            var Base64 = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(ms.GetBuffer()));
            return Base64;
        }

        //libreria   ZXing (0.16.4)
        public static string generalCodeBarras(string Value)
        {
            BarcodeWriter br = new BarcodeWriter();
            br.Format = BarcodeFormat.CODE_128;
            Bitmap bm = new Bitmap(br.Write(Value), 300, 300);

            using (var stream = new MemoryStream())
            {
                bm.Save(stream, ImageFormat.Png);
                var Base64 = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(stream.GetBuffer()));
                return Base64;
            }
        }

        public static string CreateQrCode(string content)
        {
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = 100,
                    Height = 100,
                }
            };

            var qrCodeImage = writer.Write(content); // BOOM!!

            using (var stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, ImageFormat.Png);
                var Base64 = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(stream.GetBuffer()));
                return Base64;
                //return stream.ToArray(); //return byte[]
            }
        }

        public static string ReadQrCode(byte[] qrCode)
        {
            ZXing.Windows.Compatibility.BarcodeReader coreCompatReader = new ZXing.Windows.Compatibility.BarcodeReader();

            using (Stream stream = new MemoryStream(qrCode))
            {
                using (var coreCompatImage = (Bitmap)Image.FromStream(stream))
                {
                    return coreCompatReader.Decode(coreCompatImage).Text;
                }
            }
        }

    }
}
