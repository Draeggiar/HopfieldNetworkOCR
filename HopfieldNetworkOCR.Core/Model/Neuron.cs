using System;
using System.Diagnostics;

namespace HopfieldNetworkOCR.Core.Model
{
    public class Neuron : IComparable
    {
        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                OldValue = _value;
                _value = value;
            }
        }

        public int OldValue { get; private set; }

        public int CompareTo(object obj)
        {
            var neuron = obj as Neuron;

            Debug.Assert(neuron != null, "neuron != null");
            if (neuron.Value > Value)
            {
                return 1;
            }
            if (neuron.Value < Value)
            {
                return -1;
            }
            return 0;
        }
    }
}
