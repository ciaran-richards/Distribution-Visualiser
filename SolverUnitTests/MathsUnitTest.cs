using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProbabilitySolver.Maths;

namespace SolverUnitTests
{
    [TestClass]
    public class MathsUnitTest
    {
        [TestMethod]
        public void ZeroFactorialIsOne()
        {
            var f0 = Maths.Factorial(0);
            Assert.AreEqual(f0, 1);
        }


        [TestMethod]
        public void OneFactorialIsOne()
        {
            var f1 = Maths.Factorial(0);
            Assert.AreEqual(f1, 1);
        }

        [TestMethod]
        [Description("Verify n! == n(n-1)!")]
        public void NextFactorialFactorIsCorrect()
        {
            var testCases = new List<uint>(){3,6,23};
            foreach (var input in testCases)
            {
                var control = Maths.Factorial(input);
                var test = Maths.Factorial(input - 1) * input;
                Assert.AreEqual(control,test);
            }
        }

        [TestMethod]
        [Description("Method matches pre-calculated values to eight significant figures.")]
        public void FactorialSamplesAreCorrect()
        {
            var testCases = new List<uint>() { 11, 47, 66 };
            var requiredResult = new List<double>(){39916800d, 2.5862324e+59, 5.4434494e+92};
            double threshold = 1E-8;
            for (int i = 0; i<requiredResult.Count; i++)
            {
                var test = Maths.Factorial(testCases[i]);
                var delta = (requiredResult[i] - test)/requiredResult[i];

                Assert.IsTrue(Math.Abs(delta) < threshold);
            }
        }


        [TestMethod]
        [Description("B(N,0) == 1 for any N, as is B(N,N)")]
        public void B_N0IsOne()
        {
            var testCases = new List<uint>() { 5, 14, 19 };
            foreach (var input in testCases)
            { 
                var test = Maths.BiCoef(input,0);
                Assert.AreEqual(1, test);
                test = Maths.BiCoef(input, input);
                Assert.AreEqual(1,test);
            }
        }

        [TestMethod]
        [Description("B(N,1) == N for any N, as is B(N,N-1)")]
        public void B_N1IsN()
        {
            var testCases = new List<uint>() {9, 22, 43 };
            foreach (var input in testCases)
            {
                var test = Maths.BiCoef(input, 1);
                Assert.AreEqual(input, test);
                test = Maths.BiCoef(input, input-1);
                Assert.AreEqual(input, test);
            }
        }


        [TestMethod]
        [Description("Binomial Coefficient matches pre-calculated values to 8 significant digits")]
        public void BinomialCoefficientSamplesAreCorrect()
        {
            var testCases = new List<List<uint>>()
            {
                new List<uint>(){5,3},
                new List<uint>(){17,6},
                new List<uint>(){67,34},
            };
            var requiredResult = new List<double>() {10, 12376, 14226520737620294000};
            double threshold = 1E-8;
            for (int i=0;i<testCases.Count;i++)
            {
                var test = Maths.BiCoef(testCases[i][0], testCases[i][1]);
                var delta = (requiredResult[i]-test)/requiredResult[i];
                Assert.IsTrue(delta < threshold);
            }
        }

    }
}
