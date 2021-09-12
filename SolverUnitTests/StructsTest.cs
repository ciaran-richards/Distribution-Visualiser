using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProbabilitySolver.Structs;

namespace SolverUnitTests
{
    [TestClass]
    public class StructsTest
    {
        [TestMethod]
        public void SmallNumbersAreLessThan99()
        {
            var validInputs = new List<uint>(){0, 7, 46, 85, 98};
            var invalidInputs = new List<uint>() { 99, 328, 10008 };
            foreach (var input in validInputs)
            {
                Assert.IsTrue(Small.CreateSmall(input).IsSuccess);
            }
            foreach (var input in invalidInputs)
            {
                Assert.IsTrue(Small.CreateSmall(input).IsFailure);
            }
        }

        [TestMethod]
        public void ProbabilitiesAreBetweenZeroAndOne()
        {
            var validInputs = new List<double>() { 0, 0.46, 0.628, 0.99, 1 };
            var invalidInputs = new List<double>() { -200, -0.5, 1.001, 865 };
            foreach (var input in validInputs)
            {
                Assert.IsTrue(Probability.Create(input).IsSuccess);
            }
            foreach (var input in invalidInputs)
            {
                Assert.IsTrue(Probability.Create(input).IsFailure);
            }
        }
    }
}
