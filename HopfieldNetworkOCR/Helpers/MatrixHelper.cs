using System;
using System.Collections.Generic;
using HopfieldNetworkOCR.Model;

namespace HopfieldNetworkOCR.Helpers
{
    public static class MatrixHelper
    {
        public static Matrix ImagesToMatrix(List<string> images)
        {
            if (images == null) throw new ArgumentNullException();

            var matrix = new Matrix(images[0]);

            for (int i =1; i<images.Count; i++)
                matrix.Add(new Matrix(images[i]));

            return matrix;
        }

        //public static string MatrixToImage(Matrix matrix)
        //{
        //    var imageContent = new StringBuilder();

        //    for (int i = 0; i < matrix.Size; i++)
        //    {
        //        for (int j = 0; j < matrix.Size; j++)
        //        {
                    
        //        }
        //    }

        //    return imageContent.ToString();
        //}
    }
}