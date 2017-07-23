using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace HopfieldNetworkOCR.Helpers
{
    public static class ImageHelper
    {
        public static byte[] LoadImage(string imagePath)
        {
            var img = Image.FromFile(imagePath);
            byte[] bytes;

            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                bytes = ms.ToArray();
            }

            return bytes;
        }

        public static void SaveImage(byte [] imageStream, string imagePath)
        {
            using (var ms = new MemoryStream(imageStream))
            {
                var img = Image.FromStream(ms);
                img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Tiff);
            }
        }

        public static List<byte[]> LoadAllFromCatalog(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}