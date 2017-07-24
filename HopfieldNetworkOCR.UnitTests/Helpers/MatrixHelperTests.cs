using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HopfieldNetworkOCR.UnitTests.Helpers
{
    [TestClass]
    public class MatrixHelperTests
    {
        private const string Pattern1 = "01101";
        private const string Pattern2 = "10101";

        [TestMethod]
        [TestCategory("Helpers")]
        public void ImagesToMatrixTest1()
        {
            var good = new int[5, 5] { { 0, -1, -1, 1, -1},
                                       {-1, 0, 1, -1, 1},
                                       {-1, 1, 0, -1, 1},
                                       {1, -1, -1, 0, -1},
                                       {-1, 1, 1, -1, 0} };

            var testMatrix = HopfieldNetworkOCR.Helpers.MatrixHelper.ImagesToMatrix(new System.Collections.Generic.List<string> { Pattern1 });

            Assert.AreEqual(Pattern1.Length, testMatrix.Size);

            for (int i = 0; i<Pattern1.Length; i++)
            {
                for (int j=0; j<Pattern1.Length; j++)
                {
                    Assert.AreEqual(good[i, j], testMatrix[i, j]);
                }
            }
        }

        [TestMethod]
        [TestCategory("Helpers")]
        public void ImagesToMatrixTest2()
        {
            var good = new int[5, 5] { { 0, -2, 0, 0, 0},
                                       {-2, 0, 0, 0, 0},
                                       {0, 0, 0, -2, 2},
                                       {0, 0, -2, 0, -2},
                                       {0, 0, 2, -2, 0} };

            var testMatrix = HopfieldNetworkOCR.Helpers.MatrixHelper.ImagesToMatrix(new System.Collections.Generic.List<string> { Pattern1, Pattern2 });

            Assert.AreEqual(Pattern1.Length, testMatrix.Size);

            for (int i = 0; i < Pattern1.Length; i++)
            {
                for (int j = 0; j < Pattern1.Length; j++)
                {
                    Assert.AreEqual(good[i, j], testMatrix[i, j]);
                }
            }
        }

        [TestMethod]
        [TestCategory("Helpers")]
        public void MatrixToImageTest()
        {
            Assert.Fail();
        }
    }
}