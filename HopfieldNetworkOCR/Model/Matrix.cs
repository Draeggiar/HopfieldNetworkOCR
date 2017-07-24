using System;

namespace HopfieldNetworkOCR.Model
{
    public class Matrix
    {
        private readonly int[,] _bipolarMatrix;

        public int this[int i, int j] => _bipolarMatrix[i, j];

        public int Size { get; }

        public Matrix(string inputVector)
        {
            Size = inputVector.Length;
            _bipolarMatrix = new int[Size, Size];
            
            Initialize(inputVector);         
        }

        //public void CreateRow(byte[] row)
        //{
        //    throw new System.NotImplementedException();
        //}

        public void Add(Matrix matrixToAdd)
        {
            if (matrixToAdd.Size != Size) throw new ArgumentException("Macierze muszą mieć ten sam rozmiar");

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _bipolarMatrix[i, j] += matrixToAdd[i, j];
                }
            }

            ClearDiagonal();
        }

        //private double[] ConvertToBipolar(byte[] row)
        //{
        //    throw new System.NotImplementedException();
        //}

        public Matrix GetColumn(int col)
        {
            throw new NotImplementedException();
        }

        public Matrix Transpose(string matrix)
        {
            throw new NotImplementedException();
        }

        public Matrix GetResult(Matrix matrix1, string matrix2)
        {
            throw new NotImplementedException();
        }

        private void ClearDiagonal()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i == j)
                        _bipolarMatrix[i, j] = 0;
                }
            }
        }

        // http://web.cs.ucla.edu/~rosen/161/notes/hopfield.html
        private void Initialize(string inputVector)
        {
            for (int i = 0; i < inputVector.Length; i++)
            {
                for (int j = 0; j < inputVector.Length; j++)
                {
                    _bipolarMatrix[i, j] = (2*int.Parse(inputVector[i].ToString()) - 1)*
                                           (2*int.Parse(inputVector[j].ToString()) - 1);
                }
            }

            ClearDiagonal();
        }
    }
}