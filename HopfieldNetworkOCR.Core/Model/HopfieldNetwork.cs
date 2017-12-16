using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HopfieldNetworkOCR.Core.Logic;

namespace HopfieldNetworkOCR.Core.Model
{
    public class HopfieldNetwork
    {
        public event EventHandler<TrainEventArgs> OnItemProcessed;

        private Matrix _curentWeightsMatrix;
        private string _inputVector;
        private string _outputVector;
        private Dictionary<double, string> _bestPair;
        private Dictionary<string, string> _inputVectors;

        //TODO check for null
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

        public HopfieldNetwork() { }

        public HopfieldNetwork(List<string> input) : this()
        {
            Initialize(input);
            Train(input);
        }

        private void Initialize(List<string> input)
        {
            _inputVector = input.First();
            _curentWeightsMatrix = new Matrix(input.First());
            _outputVector = input.First();
        }

        public int GetNeuronValue(int i, int j)
        {
            return _curentWeightsMatrix[i, j].Value;
        }

        public void Train(Dictionary<string, string> inputVectorsWithNames)
        {
            _inputVectors = inputVectorsWithNames;

            Train(inputVectorsWithNames.Values.ToList());
        }

        // TODO improve capacity
        public void Train(List<string> inputVectors)
        {
            if (_curentWeightsMatrix == null)
                Initialize(inputVectors);

            var trainEventArgs = new TrainEventArgs(_curentWeightsMatrix.Size);

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
                    _curentWeightsMatrix[i, j].Value = (int) (weights * 0.9);
                }
                trainEventArgs.CurrentItem = i + 1;
                OnItemProcessed?.Invoke(this, trainEventArgs);
            }

            _curentWeightsMatrix.ClearDiagonal();
        }

        // https://www.tutorialspoint.com/artificial_neural_network/artificial_neural_network_hopfield.htm
        public string GetResult(string input)
        {
            if (_bestPair == null)
                _bestPair = new Dictionary<double, string> { { double.MaxValue, string.Empty } };

            if (_curentWeightsMatrix.Size != input.Length) throw new ArgumentException("Wrong image size");

            var savedEnergyState = CurrentEnergyState;
            _inputVector = input;
            var output = new StringBuilder(input);

            var nodeToUpdate = NodeToUpdate;
            output[nodeToUpdate] = CalculateOutput(input, nodeToUpdate);
            _outputVector = output.ToString();

            if (CurrentEnergyState <= _bestPair.First().Key)
            {
                _bestPair.Clear();
                _bestPair.Add(CurrentEnergyState, _outputVector);
            }

            if (_outputVector != input 
                || CurrentEnergyState < savedEnergyState
                || _nodesToUpdate.Count > 0)
                GetResult(_outputVector);

            if (!FinalCheck(_outputVector))
                GetResult(_outputVector);
            return _bestPair.First().Value;
        }

        public bool TryGetChar(string input, out char recognizedCharacter)
        {
            recognizedCharacter = '\0';
            var isKnownChar = false;

            foreach (var vector in _inputVectors)
            {
                if (!FuzzyStringComparer.Equals(input, vector.Value)) continue;
                isKnownChar = true;
                recognizedCharacter = char.Parse(vector.Key);
            }

            return isKnownChar;
        }

        private char CalculateOutput(string input, int nodeToUpdate)
        {
            var bipolarValue = _curentWeightsMatrix.GetValueForNode(input, nodeToUpdate);

            if (bipolarValue > 0)
                return '1';
            else if (bipolarValue < 0)
                return '0';
            return input[nodeToUpdate];
        }

        private bool FinalCheck(string vectorToCheck)
        {
            var output = new StringBuilder(vectorToCheck);

            for (int i = 0; i < _curentWeightsMatrix.Size; i++)
            {
                output[i] = CalculateOutput(vectorToCheck, i);
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

        public void ResetNetworkState()
        {
            _bestPair.Clear();
            _bestPair.Add(double.MaxValue, string.Empty);
            _nodesToUpdate = null;
        }
    }

    public class TrainEventArgs : EventArgs
    {
        public int ItemsCount;
        public int CurrentItem;

        internal TrainEventArgs(int items)
        {
            ItemsCount = items;
        }
    }
}