using System;
using System.Collections.Generic;
using HopfieldNetworkOCR.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HopfieldNetworkOCR.UnitTests.Model
{
    [TestClass]
    public class HopfieldNetworkTests
    {


        [TestMethod]
        [TestCategory("HopfieldNetwork")]
        public void TrainTest()
        {
            var good = new int[5, 5] { { 0, -2, 0, 0, 0},
                                    {-2, 0, 0, 0, 0},
                                    {0, 0, 0, -2, 2},
                                    {0, 0, -2, 0, -2},
                                    {0, 0, 2, -2, 0} };

            var newtork = new HopfieldNetwork(Helpers.MatrixHelperTests.Pattern1);
            newtork.Train(new List<string>{Helpers.MatrixHelperTests.Pattern2});

            var matrixSize = Math.Sqrt(newtork.NumberOfNeurons);

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    Assert.AreEqual(good[i, j], newtork.GetNeuronValue(i, j));
                }
            }
        }

        [TestMethod]
        [TestCategory("HopfieldNetwork")]
        public void GetResult()
        {
            var patternsToTrain = new List<string>
            {
                Helpers.MatrixHelperTests.Pattern2
            };
        
            var network = new HopfieldNetwork(Helpers.MatrixHelperTests.Pattern1);
            network.Train(patternsToTrain);
            var result = network.GetResult(Helpers.MatrixHelperTests.Pattern3);

            Assert.AreEqual(Helpers.MatrixHelperTests.Pattern1, result);
        }

        [TestMethod()]
        [TestCategory("HopfieldNetwork")]
        public void EvaluateEnergyFunctionTest()
        {
            var network = new HopfieldNetwork(Helpers.MatrixHelperTests.Pattern1);
            network.Train(new List<string>{Helpers.MatrixHelperTests.Pattern2});

            var energyState = network.CurrentEnergyState;

            network.GetResult(Helpers.MatrixHelperTests.Pattern3);

            Assert.IsTrue(network.CurrentEnergyState < energyState);
        }
    }
}