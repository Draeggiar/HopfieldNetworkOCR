using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace HopfieldNetworkOCR.Helpers
{
    public static class ImageHelper
    {
        private const int ResultImageWidth = 10;
        private const int ResultImageHeight = 12;

        public static string LoadImage(string imagePath)
        {
            var imageContent = new StringBuilder();
            var img = new Bitmap(imagePath);

            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    var pixelColor = img.GetPixel(j, i);
                    if (pixelColor.ToArgb() == Color.Black.ToArgb())
                    {
                        imageContent.Append("1");
                    }
                    else if (pixelColor.ToArgb() == Color.White.ToArgb())
                    {
                        imageContent.Append("0");
                    }
                    else
                    {
                        throw new ArgumentException("Picture is not binary");
                    }
                }
            }

            return imageContent.ToString();
        }

        public static Bitmap ConvertVectorToImage(string imageContent, int imageWidth = ResultImageWidth, int imageHeight = ResultImageHeight)
        {
            var newImage = new Bitmap(imageWidth, imageHeight);

            int pixelCount = 0;
            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    newImage.SetPixel(j, i, imageContent[pixelCount] == '0' ? Color.White : Color.Black);
                    pixelCount += 1;
                }
            }

            return newImage;
        }

        public static Dictionary<string, string> LoadAllFromCatalog(string path)
        {
            var images = new Dictionary<string, string>();
            var dirInfo = new DirectoryInfo(path);
            var filesInfo = dirInfo.GetFiles("*.tiff");

            foreach (FileInfo fileInfo in filesInfo)
            {
                images.Add(fileInfo.Name.Split('.')[0], LoadImage(path + "\\" + fileInfo.Name));
            }

            return images;
        }

        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}