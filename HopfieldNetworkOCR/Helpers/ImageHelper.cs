using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace HopfieldNetworkOCR.Helpers
{
    public static class ImageHelper
    {
        public static string LoadImage(string imagePath)
        {
            var imageContent = new StringBuilder();
            var img = new Bitmap(imagePath);

            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                  Color pixelColor = img.GetPixel(j, i);
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
                        throw new ArgumentException("Picture is not binary!");
                    }
                }
            }

            return imageContent.ToString();
        }

        public static void SaveImage(string imageContent, string imagePath, int imageWidth, int imageHeight)
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

            newImage.Save(imagePath);
        }

        public static List<string> LoadAllFromCatalog(string path)
        {
            var images = new List<string>();
            var dirInfo = new DirectoryInfo(path);
            var filesInfo = dirInfo.GetFiles("*.tiff");

            foreach (FileInfo fileInfo in filesInfo)
            {
                images.Add(LoadImage(path+  "\\" + fileInfo.Name));
            }

            return images;
        }
    }
}