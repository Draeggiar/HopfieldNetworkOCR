using System;
using System.Collections.Generic;
using HopfieldNetworkOCR.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HopfieldNetworkOCR.UnitTests.Model
{
    [TestClass]
    public class HopfieldNetworkTests
    {
        internal const string Pattern1 = "01101";
        internal const string Pattern2 = "10101";
        internal const string Pattern3 = "11111";

        [TestMethod]
        [TestCategory("Core")]
        public void TrainEventTest()
        {
            var network = new HopfieldNetwork();
            network.OnItemProcessed += (sender, args) =>
            {
                Assert.AreEqual(Math.Sqrt(network.NumberOfNeurons), args.ItemsCount);
            };
            network.Train(new Dictionary<string, string>
            {
                {"1", Pattern1},
                {"2", Pattern2 }
            });
        }

        [TestMethod]
        [TestCategory("Core")]
        public void GetResult()
        {
            var network = new HopfieldNetwork();
            network.Train(new Dictionary<string, string>
            {
                {"1", Pattern1},
                {"2", Pattern2 }
            });

            var result = network.GetResult(Pattern1);

            Assert.AreEqual(Pattern1, result);
        }

        [TestMethod]
        [TestCategory("Core")]
        public void EvaluateEnergyFunctionTest()
        {
            var network = new HopfieldNetwork();
            network.Train(new Dictionary<string, string>
            {
                {"1", Pattern1},
                {"2", Pattern2 }
            });

            var energyState = network.CurrentEnergyState;

            network.GetResult(Pattern3);

            Assert.IsTrue(network.CurrentEnergyState <= energyState);
        }
    }
}