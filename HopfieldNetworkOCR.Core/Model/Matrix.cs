namespace HopfieldNetworkOCR.Core.Model
{
    public class Matrix
    {
        private readonly double[,] _weightMatrix;

        public double this[int i, int j]
        {
            get { return _weightMatrix[i, j]; }
            set { _weightMatrix[i, j] = value; }
        }

        public int Size { get; }

        public Matrix(string inputVector)
        {
            Size = inputVector.Length;
            _weightMatrix = new double[Size, Size];

            Initialize(inputVector);
        }

        // http://web.cs.ucla.edu/~rosen/161/notes/hopfield.html
        private void Initialize(string inputVector)
        {
            for (int i = 0; i < inputVector.Length; i++)
            {
                for (int j = 0; j < inputVector.Length; j++)
                {
                    if (i == j)
                        _weightMatrix[i, j] = 0;
                    else
                        _weightMatrix[i, j] = (2 * int.Parse(inputVector[i].ToString()) - 1) *
                                              (2 * int.Parse(inputVector[j].ToString()) - 1);
                }
            }
        }

        public double GetValueForNode(string input, int nodeToUpdate)
        {
            var newNodeValue = int.Parse(input[0].ToString()) * this[nodeToUpdate, 0];
            for (int j = 1; j < Size ; j++)
            {
                newNodeValue += int.Parse(input[j].ToString()) * this[nodeToUpdate, j];
            }
            return newNodeValue;
        }
    }
}