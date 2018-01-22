using HopfieldNetworkOCR.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HopfieldNetworkOCR.UnitTests.Model
{
    [TestClass]
    public class MatrixTests
    {
        [TestMethod]
        [TestCategory("Core")]
        public void MatrixCreationTest()
        {
            var good = new int[5, 5] { { 0, -1, -1, 1, -1},
                                        {-1, 0, 1, -1, 1},
                                        {-1, 1, 0, -1, 1},
                                        {1, -1, -1, 0, -1},
                                        {-1, 1, 1, -1, 0}};
            var matrix = new Matrix(HopfieldNetworkTests.Pattern1);

            Assert.AreEqual(HopfieldNetworkTests.Pattern1.Length, matrix.Size);

            for (int i = 0; i < matrix.Size; i++)
            {
                for (int j = 0; j < matrix.Size; j++)
                {
                    Assert.AreEqual(good[i, j], matrix[i, j]);
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void GetValueForNodeTest()
        {
            var matrix1 = new Matrix(HopfieldNetworkTests.Pattern1);
            var matrix2 = new Matrix(HopfieldNetworkTests.Pattern2);

            for (int i = 0; i < matrix1.Size; i++)
            {
                for (int j = 0; j < matrix1.Size; j++)
                {
                    matrix1[i, j] += matrix2[i, j];
                }
            }

            var result = matrix1.GetValueForNode(HopfieldNetworkTests.Pattern3, 2) >= 0 ? '1' : '0';

            Assert.AreEqual('1', result);
        }
    }
}