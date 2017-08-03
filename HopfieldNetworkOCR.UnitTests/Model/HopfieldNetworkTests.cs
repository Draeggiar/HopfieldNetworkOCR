using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HopfieldNetworkOCR.UnitTests.Model
{
    [TestClass]
    public class HopfieldNetworkTests
    {
        [TestMethod]
        public void TrainTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestCategory("HopfieldNetwork")]
        public void GetResult()
        {
            var matrix = HopfieldNetworkOCR.Helpers.MatrixHelper.ImagesToMatrix(new List<string>
            {
                Helpers.MatrixHelperTests.Pattern1,
                Helpers.MatrixHelperTests.Pattern2
            });

            var network = new HopfieldNetworkOCR.Model.HopfieldNetwork(matrix);
            var result = network.GetResult("11111");

            Assert.AreEqual(Helpers.MatrixHelperTests.Pattern1, result);
        }
    }
}