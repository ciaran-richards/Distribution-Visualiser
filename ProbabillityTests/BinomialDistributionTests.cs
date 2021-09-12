using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProbabilitySolver;
using ProbabilitySolver.Structs;

namespace ProbabillityTests
{
    [TestClass]
    public class BinomialDistributionTests
    {
        [TestMethod]
        public void DistributionsSumToOne()
        {
            var b = Small.CreateSmall(5).Value;
            var c = Probability.Create(0.3).Value;
            var a = new BinomialDistribution(b, c);
            Assert.AreEqual(1d, a.SumProbability[5], 0.000001);
        }
        [TestMethod]
        public void DistributionsSumAlwaysIncreases()
        {
            var b = Small.CreateSmall(7).Value;
            var c = Probability.Create(0.25).Value;
            var a = new BinomialDistribution(b, c);
            for (uint i = 1; i <= 7; i++)
            {
                Assert.IsTrue(a.SumProbability[i] >= a.SumProbability[i-1]);
            }
        }
    }
}
