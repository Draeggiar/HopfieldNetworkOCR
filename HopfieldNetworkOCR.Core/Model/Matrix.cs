using System;

namespace HopfieldNetworkOCR.Core.Model
{
    public class Matrix
    {
        private readonly Neuron[,] _bipolarMatrix;

        public Neuron this[int i, int j] => _bipolarMatrix[i, j];

        public int Size { get; }

        public Matrix(string inputVector)
        {
            Size = inputVector.Length;
            _bipolarMatrix = new Neuron[Size, Size];
            
            Initialize(inputVector);         
        }

        //public void CreateRow(byte[] row)
        //{
        //    throw new System.NotImplementedException();
        //}

        public void Add(Matrix matrixToAdd)
        {
            if (matrixToAdd.Size != Size) throw new ArgumentException("Matrixes must have same size");

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _bipolarMatrix[i, j].Value += matrixToAdd[i, j].Value;
                }
            }

            ClearDiagonal();
        }

        public int GetValueForNode(string input, int nodeToUpdate)
        {
            int newNodeValue = int.Parse(input[0].ToString()) * this[0, nodeToUpdate].Value;
            for (int j = 1; j < Size ; j++)
            {
                newNodeValue += int.Parse(input[j].ToString()) * this[j, nodeToUpdate].Value;
            }
            return newNodeValue;
        }

        //private double[] ConvertToBipolar(byte[] row)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public Matrix GetColumn(int col)
        //{
        //    throw new NotImplementedException();
        //}

        //public Matrix Transpose(string matrix)
        //{
        //    throw new NotImplementedException();
        //}

        private void ClearDiagonal()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i == j)
                        _bipolarMatrix[i, j].Value = 0;
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
                    _bipolarMatrix[i, j] = new Neuron
                    {
                        Value = (2*int.Parse(inputVector[i].ToString()) - 1)*
                                (2*int.Parse(inputVector[j].ToString()) - 1)
                    };
                }
            }

            ClearDiagonal();
        }
    }
}