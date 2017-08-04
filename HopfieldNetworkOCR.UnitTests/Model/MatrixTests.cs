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

            var matrix = new Matrix(Helpers.MatrixHelperTests.Pattern1);

            Assert.AreEqual(Helpers.MatrixHelperTests.Pattern1.Length, matrix.Size);

            for (int i = 0; i < matrix.Size; i++)
            {
                for (int j = 0; j < matrix.Size; j++)
                {
                    Assert.AreEqual(good[i, j], matrix[i, j].Value);
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void GetValueForNodeTest()
        {
            var matrix = new Matrix(Helpers.MatrixHelperTests.Pattern1);
            matrix.Add(new Matrix(Helpers.MatrixHelperTests.Pattern2));

            var result = matrix.GetValueForNode(Helpers.MatrixHelperTests.Pattern3, 2) >= 0 ? '1' : '0';

            Assert.AreEqual('1', result);
        }
    }
}