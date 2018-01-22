using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HopfieldNetworkOCR.Core.Logic;

namespace HopfieldNetworkOCR.Core.Model
{
    public class HopfieldNetwork
    {
        public event EventHandler<ProcessEventArgs> OnItemProcessed;

        private Matrix _curentWeightsMatrix;
        private string _inputVector;
        private string _outputVector;
        private KeyValuePair<double, string> _localMinimum;
        private Dictionary<string, string> _inputVectorsWithNames;
        private int _iteration;

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

        public void Train(Dictionary<string, string> inputVectorsWithNames)
        {
            _inputVectorsWithNames = inputVectorsWithNames;

            var vectors = inputVectorsWithNames.Values.ToList();
            Initialize(vectors.First());
            HebbianLearning(vectors);
        }

        private void Initialize(string input)
        {
            _inputVector = input;
            _curentWeightsMatrix = new Matrix(input);
            _outputVector = input;
            _localMinimum = new KeyValuePair<double, string>(double.MaxValue, string.Empty);
        }

        private void HebbianLearning(List<string> inputVectors)
        {
            var trainEventArgs = new ProcessEventArgs(_curentWeightsMatrix.Size);

            // Hebbian Rule
            for (int i = 0; i < _curentWeightsMatrix.Size; i++)
            {
                for (int j = 0; j < _curentWeightsMatrix.Size; j++)
                {
                    var weights = 0.0;
                    if (i != j)
                    {
                        foreach (string vector in inputVectors)
                        {
                            weights += (2 * double.Parse(vector[i].ToString()) - 1) *
                                       (2 * double.Parse(vector[j].ToString()) - 1);
                        }
                    }
                    _curentWeightsMatrix[i, j] = weights / _curentWeightsMatrix.Size;
                }
                trainEventArgs.CurrentItem = i + 1;
                OnItemProcessed?.Invoke(this, trainEventArgs);
            }
        }

        // https://www.tutorialspoint.com/artificial_neural_network/artificial_neural_network_hopfield.htm
        public string GetResult(string input)
        {
            if (_curentWeightsMatrix.Size != input.Length)
                throw new ArgumentException("Wrong image size");

            OnItemProcessed?.Invoke(this, 
                new ProcessEventArgs(int.MaxValue) {CurrentItem = ++_iteration});

            _inputVector = input;
            var savedEnergyState = CurrentEnergyState;
            var output = new StringBuilder(input);

            var nodeToUpdate = NodeToUpdate;
            output[nodeToUpdate] = ApplyActivationFunction(input, nodeToUpdate);
            _outputVector = output.ToString();

            if (CurrentEnergyState < _localMinimum.Key)
                _localMinimum = new KeyValuePair<double, string>(CurrentEnergyState, _outputVector);

            if (CurrentEnergyState > savedEnergyState
                || _nodesToUpdate.Count > 0
                && !FinalCheck(_outputVector))
                GetResult(_outputVector);

            return _localMinimum.Value;
        }

        public bool TryGetChar(string input, out char recognizedCharacter)
        {
            recognizedCharacter = '\0';
            var isKnownChar = false;

            foreach (var vector in _inputVectorsWithNames)
            {
                if (string.IsNullOrEmpty(vector.Key) || vector.Key.Length > 1)
                    throw new ArgumentException("Cannot associate output with letter. Check file names.");

                if(!string.Equals(input, vector.Value))
                    if (!FuzzyStringComparer.Equals(input, vector.Value)) continue;
                isKnownChar = true;
                recognizedCharacter = char.Parse(vector.Key);
                break;
            }

            return isKnownChar;
        }

        private char ApplyActivationFunction(string input, int nodeToUpdate)
        {
            var neuronWeight = _curentWeightsMatrix.GetValueForNode(input, nodeToUpdate);

            if (neuronWeight > 0)
                return '1';
            else if(neuronWeight < 0)
                return '0';
            return input[nodeToUpdate];
        }

        private bool FinalCheck(string vectorToCheck)
        {
            var output = new StringBuilder(vectorToCheck);

            for (int i = 0; i < _curentWeightsMatrix.Size; i++)
            {
                output[i] = ApplyActivationFunction(vectorToCheck, i);
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
                    energy += double.Parse(_outputVector[i].ToString()) 
                        * double.Parse(_outputVector[j].ToString()) 
                        * _curentWeightsMatrix[i, j];
                }
            }

            energy = energy *  -(1.0 / 2.0);

            for (int i = 0; i < _curentWeightsMatrix.Size; i++)
            {
                energy -= double.Parse(_inputVector[i].ToString()) 
                    * double.Parse(_outputVector[i].ToString());
            }

            return energy;
        }

        public void ResetNetworkState()
        {
            _localMinimum = new KeyValuePair<double, string>(double.MaxValue, string.Empty);
            _nodesToUpdate = null;
            _iteration = 0;
        }
    }

    public class ProcessEventArgs : EventArgs
    {
        public int ItemsCount;
        public int CurrentItem;

        internal ProcessEventArgs(int items)
        {
            ItemsCount = items;
        }
    }
}