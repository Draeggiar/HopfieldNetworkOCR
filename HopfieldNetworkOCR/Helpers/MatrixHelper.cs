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

        public static byte[] MatrixToImage(Matrix matrix)
        {
            throw new System.NotImplementedException();
        }
    }
}