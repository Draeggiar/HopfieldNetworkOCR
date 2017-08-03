using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HopfieldNetworkOCR.Model
{
    public class HopfieldNetwork
    {
        private readonly Matrix _curentWeightsMatrix;

        private List<int> _nodesToUpdate;

        private int NodeToUpdate
        {
            get
            {
                if (_nodesToUpdate == null || _nodesToUpdate.Count == 0)
                    _nodesToUpdate = new List<int>(Enumerable.Range(0, _curentWeightsMatrix.Size));

                var rnd = new Random(DateTime.Now.Millisecond);
                var index = rnd.Next(0, _nodesToUpdate.Count - 1);
                var nodeToUpdate = _nodesToUpdate[index];

                _nodesToUpdate.RemoveAt(index);
                return nodeToUpdate;
            }
        }

        private int NumberOfNeurons => _curentWeightsMatrix.Size * _curentWeightsMatrix.Size;

        public HopfieldNetwork(Matrix weightMatrix)
        {
            _curentWeightsMatrix = weightMatrix;
        }

        public void Train()
        {
            throw new System.NotImplementedException();
        }

        // https://www.tutorialspoint.com/artificial_neural_network/artificial_neural_network_hopfield.htm
        public string GetResult(string input)
        {
            if (_curentWeightsMatrix.Size != input.Length) throw new ArgumentException("Wrong image size");

            var output = new StringBuilder(input);

            var nodeToUpdate = NodeToUpdate;

            int newNodeValue = int.Parse(input[0].ToString()) * _curentWeightsMatrix[0, nodeToUpdate].Value;
            for (int j = 1; j < _curentWeightsMatrix.Size; j++)
            {
                newNodeValue += int.Parse(input[j].ToString()) * _curentWeightsMatrix[j, nodeToUpdate].Value;
            }

            output[nodeToUpdate] = newNodeValue >= 0 ? '1' : '0';

            //TODO change end condition 
            if (output.ToString() != input || _nodesToUpdate.Count > 0)
                GetResult(output.ToString());
            
            return output.ToString();
        }
    }
}