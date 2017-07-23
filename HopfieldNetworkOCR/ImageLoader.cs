using System;
using System.Collections.Generic;
using System.Drawing;
using Color = System.Drawing.Color;

namespace HopfieldNetworkOCR
{
    class ImageLoader
    {
        public List<bool> Load(string imagePath)
        {
            List<bool> pixelValues = new List<bool>();
            Bitmap img = new Bitmap(imagePath);

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixelColor = img.GetPixel(i, j);
                    if (pixelColor == Color.Black)
                    {
                        pixelValues.Add(false);
                    }
                    else if (pixelColor == Color.White)
                    {
                        pixelValues.Add(true);
                    }
                    else
                    {
                        throw new ArgumentException("Obraz nie jest binarny");
                    }
                }
            }

            return pixelValues;
        }
    }
}
