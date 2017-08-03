using System.Collections.Generic;
using HopfieldNetworkOCR.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HopfieldNetworkOCR.UnitTests.Model
{
    [TestClass]
    public class MatrixTests
    {
        [TestMethod]
        public void GetValueForNodeTest()
        {
            var matrix = new Matrix(Helpers.MatrixHelperTests.Pattern1);
            matrix.Add(new Matrix(Helpers.MatrixHelperTests.Pattern2));

            var result = matrix.GetValueForNode(Helpers.MatrixHelperTests.Pattern3, 3) >= 0 ? '1' : '0';

            Assert.AreEqual('1', result);
        }
    }
}