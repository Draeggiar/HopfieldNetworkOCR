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
        [TestCategory("Core")]
        public void TrainTest()
        {
            //Matrix for pattern1 
            var good = new int[5, 5] { { 0, -1, -1, 1, -1},
                                        {-1, 0, 1, -1, 1},
                                        {-1, 1, 0, -1, 1},
                                        {1, -1, -1, 0, -1},
                                        {-1, 1, 1, -1, 0}};
            var network = new HopfieldNetwork(new List<string> {Helpers.MatrixHelperTests.Pattern2 });

            //Hebbian rule
            network.Train(new List<string> { Helpers.MatrixHelperTests.Pattern1});
            var matrixSize = Math.Sqrt(network.NumberOfNeurons);

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    Assert.AreEqual(good[i, j], network.GetNeuronValue(i, j));
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void TrainEventTest()
        {
            var network = new HopfieldNetwork(new List<string> { Helpers.MatrixHelperTests.Pattern1 });
            network.OnItemProcessed += (sender, args) =>
            {
                Assert.AreEqual(Math.Sqrt(network.NumberOfNeurons), args.ItemsCount);
            };
            network.Train(new List<string> { Helpers.MatrixHelperTests.Pattern2 });
        }

        [TestMethod]
        [TestCategory("Core")]
        public void GetResult()
        {
            var network = new HopfieldNetwork(new List<string> { Helpers.MatrixHelperTests.Pattern1,
                Helpers.MatrixHelperTests.Pattern2 });

            var result = network.GetResult(Helpers.MatrixHelperTests.Pattern1);

            Assert.AreEqual(Helpers.MatrixHelperTests.Pattern1, result);
        }

        [TestMethod]
        [TestCategory("Core")]
        public void EvaluateEnergyFunctionTest()
        {
            var network = new HopfieldNetwork(new List<string> { Helpers.MatrixHelperTests.Pattern1 });
            network.Train(new List<string>{Helpers.MatrixHelperTests.Pattern2});

            var energyState = network.CurrentEnergyState;

            network.GetResult(Helpers.MatrixHelperTests.Pattern3);

            Assert.IsTrue(network.CurrentEnergyState <= energyState);
        }
    }
}