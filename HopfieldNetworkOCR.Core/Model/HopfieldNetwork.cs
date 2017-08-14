using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HopfieldNetworkOCR.Core.Model
{
    public class HopfieldNetwork
    {
        private Matrix _curentWeightsMatrix;
        private string _inputVector;
        private Dictionary<double, string> _bestPair;

        public int NumberOfNeurons => _curentWeightsMatrix.Size * _curentWeightsMatrix.Size;
        public double CurrentEnergyState => EvaluateEnergyFunction();

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

        private string _outputVector;
        //public string OutputVector => GetResult(_inputVector)

        public HopfieldNetwork() { }

        public HopfieldNetwork(List<string> input)
        {
            if (string.IsNullOrEmpty(_inputVector) && string.IsNullOrEmpty(_outputVector))
            {
                _inputVector = input.First();
                _curentWeightsMatrix = new Matrix(input.First());
                _outputVector = input.First();
                _bestPair = new Dictionary<double, string> { { CurrentEnergyState, input.First() } };
            }

            Train(input);
        }

        public int GetNeuronValue(int i, int j)
        {
            return _curentWeightsMatrix[i, j].Value;
        }

        public void Train(List<string> inputVectors)
        {
            // Hebbian Rule

            for (int i = 0; i < _curentWeightsMatrix.Size; i++)
            {
                for (int j = 0; j < _curentWeightsMatrix.Size; j++)
                {
                    int weights = 0;
                    foreach (string vector in inputVectors)
                    {
                        weights += (2 * int.Parse(vector[i].ToString()) - 1) *
                                   (2 * int.Parse(vector[j].ToString()) - 1);
                    }
                    _curentWeightsMatrix[i, j].Value = weights;
                }
            }

            _curentWeightsMatrix.ClearDiagonal();
        }

        // https://www.tutorialspoint.com/artificial_neural_network/artificial_neural_network_hopfield.htm
        public string GetResult(string input)
        {
            while (true)
            {
                if (_curentWeightsMatrix.Size != input.Length) throw new ArgumentException("Wrong image size");

                var savedEnergyState = CurrentEnergyState;
                _inputVector = input;
                var output = new StringBuilder(input);

                var nodeToUpdate = NodeToUpdate;
                output[nodeToUpdate] = _curentWeightsMatrix.GetValueForNode(input, nodeToUpdate) >= 0 ? '1' : '0';
                _outputVector = output.ToString();

                if (CurrentEnergyState < _bestPair.First().Key)
                {
                    _bestPair.Clear();
                    _bestPair.Add(CurrentEnergyState, _outputVector);
                }

                if (_outputVector != input && CurrentEnergyState< savedEnergyState)
                    GetResult(_outputVector);

                if (FinalCheck(_outputVector))
                    break;

                input = _outputVector;
            }
            return _bestPair.First().Value;
        }

        private bool FinalCheck(string vectorToCheck)
        {
            var output = new StringBuilder(vectorToCheck);

            for (int i = 0; i < _curentWeightsMatrix.Size; i++)
            {
                output[i] = _curentWeightsMatrix.GetValueForNode(vectorToCheck, i) >= 0 ? '1' : '0';
                if (output.ToString() != vectorToCheck)
                    return false;
            }
            return true;
        }

        // http://www.comp.leeds.ac.uk/ai23/reading/Hopfield.pdf
        private double EvaluateEnergyFunction()
        {
            double energy = 0;

            for (int i = 0; i < _curentWeightsMatrix.Size; i++)
            {
                for (int j = 0; j < _curentWeightsMatrix.Size; j++)
                {
                    energy += int.Parse(_outputVector[i].ToString()) * int.Parse(_outputVector[j].ToString()) *
                              _curentWeightsMatrix[i, j].Value;
                }
            }

            energy = energy * -(1 / 2);

            for (int i = 0; i < _curentWeightsMatrix.Size; i++)
            {
                energy -= _inputVector[i] * _outputVector[i];
            }

            return energy;
        }
    }
}