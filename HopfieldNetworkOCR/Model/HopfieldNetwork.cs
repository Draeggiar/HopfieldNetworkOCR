using System;
using System.Text;

namespace HopfieldNetworkOCR.Model
{
    public class HopfieldNetwork
    {
        private readonly Matrix _curentWeightsMatrix;
        //private int _iterations;

        private int NumberOfNeurons => _curentWeightsMatrix.Size * _curentWeightsMatrix.Size;

        //public int Neurons { get; set; }

        public HopfieldNetwork(Matrix weightMatrix)
        {
            _curentWeightsMatrix = weightMatrix;
        }

        public void Train()
        {
            throw new System.NotImplementedException();
        }

        // https://www.tutorialspoint.com/artificial_neural_network/artificial_neural_network_hopfield.htm
        public string GetResult(string imageToRecognize)
        {
            if(_curentWeightsMatrix.Size != imageToRecognize.Length) throw new ArgumentException("Wrong image size");

            var output = new StringBuilder();

            //TODO verify
            for (int i = 0; i < _curentWeightsMatrix.Size; i++)
            {
                int nodeValue = _curentWeightsMatrix[0, i] * int.Parse(imageToRecognize[0].ToString());
                for (int j = 1; j < _curentWeightsMatrix.Size; j++)
                {
                    nodeValue += _curentWeightsMatrix[j, i] * int.Parse(imageToRecognize[j].ToString());
                }
                output.Append(nodeValue >= 0 ? "1" : "0");
            }

            return output.ToString();
        }
    }
}