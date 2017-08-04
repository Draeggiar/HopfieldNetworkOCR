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

        //public HopfieldNetwork() { }

        public HopfieldNetwork(List<string> input)
        {
            var initVector = input.First();
            input.Remove(initVector);
            Initialize(initVector);

            Train(input);
        }

        private void Initialize(string input)
        {
            _inputVector = input;
            _curentWeightsMatrix = new Matrix(input);
            _outputVector = _inputVector;
        }

        public int GetNeuronValue(int i, int j)
        {
            return _curentWeightsMatrix[i, j].Value;
        }

        public void Train(List<string> inputVectors)
        {
            foreach (string inputVector in inputVectors)
            {
                _curentWeightsMatrix.Add(new Matrix(inputVector));
            }
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

                if (_outputVector != input && savedEnergyState > CurrentEnergyState)
                    GetResult(_outputVector);

                if (FinalCheck(_outputVector))
                    return _outputVector;

                input = _outputVector;
            }
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