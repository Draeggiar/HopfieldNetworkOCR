using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HopfieldNetworkOCR
{
    public class Matrix
    {
        private double[][] _bipolarMatrix;

        public int Size
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public void CreateRow(byte[] row)
        {
            throw new System.NotImplementedException();
        }

        private double[] ConvertToBipolar(byte[] row)
        {
            throw new System.NotImplementedException();
        }

        public Matrix GetColumn(int col)
        {
            throw new System.NotImplementedException();
        }

        public Matrix Transpose(string matrix)
        {
            throw new System.NotImplementedException();
        }

        public Matrix GetResult(Matrix matrix1, string matrix2)
        {
            throw new System.NotImplementedException();
        }

        private void ClearDiagonal()
        {
            throw new System.NotImplementedException();
        }
    }
}