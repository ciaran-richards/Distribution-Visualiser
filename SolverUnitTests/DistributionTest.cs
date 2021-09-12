using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProbabilitySolver;
using ProbabilitySolver.Distributions;
using ProbabilitySolver.Maths;
using ProbabilitySolver.Structs;

namespace SolverUnitTests
{
    [TestClass]
    public class DistributionTest
    {
        private BinomialDistribution binomialCaseOne =
            new BinomialDistribution(Small.CreateSmall(20).Value, Probability.Create(0.3).Value);


        private BinomialDistribution binomialCaseTwo =
            new BinomialDistribution(Small.CreateSmall(67).Value, Probability.Create(0.9).Value);

        private NormalDistribution normalCaseOne =
            new NormalDistribution(35, 13);
        //19.96875
        //0.0157274934668589
        //0.1237895

        private NormalDistribution normalCaseTwo =
            new NormalDistribution(836826, 100);

        //836526
        //0.0000443184841193801
        //0.001349897

        private NormalDistribution normalCaseThree =
            new NormalDistribution(-70, 100);
        //5
        //0.00301137432154804
        //0.773373
        private double threshold = 1E-8;
        private double erfThreshold = 1E-6;

        [TestMethod]
        [Description("Binomial Distribution matches pre calculated values to eight significant figures")]
        public void BinomialDistributionIsCorrect()
        {

            var testOneCase = 13;
            var testOneProability = 0.0010178326;

            var test = binomialCaseOne.Probabilities[testOneCase];
            var delta = (test - testOneProability) / testOneProability;
            Assert.IsTrue(delta < threshold);

            var testTwoCase = 60;
            var testTwoProability = 0.15627667871;

            test = binomialCaseTwo.Probabilities[testTwoCase];
            delta = (test - testTwoProability) / testTwoProability;
            Assert.IsTrue(delta < threshold);
        }

        [TestMethod]
        [Description("Binomial Distribution matches pre calculated values to eight significant figures")]
        public void BinomialSumDistributionIsCorrect()
        {

            var testOneCase = 13;
            var testOneSumProbability = 0.99973895299;

            var test = binomialCaseOne.SumProbability[testOneCase];
            var delta = (test - testOneSumProbability) / testOneSumProbability;
            Assert.IsTrue(delta < threshold);

            var testTwoCase = 60;
            var testTwoSumProbability = 0.51041420595;

            test = binomialCaseTwo.SumProbability[testTwoCase];
            delta = (test - testTwoSumProbability) / testTwoSumProbability;
            Assert.IsTrue(delta < threshold);
        }

        [TestMethod]
        [Description("Normal Distribution matches pre calculated values to eight significant figures")]
        public void NormalDistributionIsCorrect()
        {

            var testOneCase = 19.96875;
            var testOneProability = 0.0157274934668589;
            //87

            var test = normalCaseOne.Probabilities[87];
            var delta = (test - testOneProability) / testOneProability;
            Assert.IsTrue(delta < threshold);

            
            var testTwoCase = 836526;
            var testTwoProability = 0.0000443184841193801;

            test = normalCaseTwo.Probabilities[2];
            delta = (test - testTwoProability) / testTwoProability;
            Assert.IsTrue(delta < threshold);

            var testThreeCase = 5;
            var testThreeProbability = 0.00301137432154804;

            test = normalCaseThree.Probabilities[349];
            delta = (test - testThreeProbability) / testThreeProbability;
            Assert.IsTrue(delta < threshold);
        }

        [TestMethod]
        [Description("Normal Distribution matches pre calculated values to eight significant figures")]
        public void NormalSumDistributionIsCorrect()
        {

            var testOneCase = 19.96875;
            var testOneSumProbability = 0.1237895;
            //87


            var test = normalCaseOne.SumProbability[87];
            var delta = (test - testOneSumProbability) / testOneSumProbability;
            Assert.IsTrue(delta < threshold);


            var testTwoCase = 836526;
            var testTwoSumProbability = 0.001349897;

            test = normalCaseTwo.SumProbability[2];
            delta = (test - testTwoSumProbability) / testTwoSumProbability;
            Assert.IsTrue(delta < threshold);

            var testThreeCase = 5;
            var testThreeSumProbability = 0.773373;

            test = normalCaseThree.SumProbability[2];
            delta = (test - testThreeSumProbability) / testThreeSumProbability;
            Assert.IsTrue(delta < threshold);

        }
    }
}
