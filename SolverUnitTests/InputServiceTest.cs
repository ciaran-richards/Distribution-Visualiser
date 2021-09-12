using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using CSharpFunctionalExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProbabilitySolver.Services.InputService;
using ProbabilitySolver.Structs;

namespace SolverUnitTests
{
    [TestClass]
    public class InputServiceTest
    {

        [TestMethod]
        public void DoubleServicesRejectBadInputs()
        {
            var inputs = new List<string>() {"30.7", "20", "2", "0", "-0.7", "", "-", "%%#@][[][]"};
            var control = new List<Result<double>>()
            {
                Result.Success<double>(30.7),
                Result.Success<double>(20),
                Result.Success<double>(2),
                Result.Success<double>(0),
                Result.Success<double>(-0.7),
                Result.Failure<double>("TestError"),
                Result.Failure<double>("TestError"),
                Result.Failure<double>("TestError")
            };
            var doubleService = new InputServiceDouble();
            Result<double> testResult;

            for (int i = 0; i < inputs.Count; i++)
            {
                testResult = doubleService.Run(inputs[i]);
                Assert.AreEqual(testResult.IsSuccess, control[i].IsSuccess);
            }
        }

        [TestMethod]
        public void DoubleServicesValuesCorrect()
        {
            var inputs = new List<string>() {"30.7", "20", "2", "0", "-0.7"};
            var control = new List<Result<double>>()
            {
                Result.Success<double>(30.7),
                Result.Success<double>(20),
                Result.Success<double>(2),
                Result.Success<double>(0),
                Result.Success<double>(-0.7)
            };
            var doubleService = new InputServiceDouble();

            for (int i = 0; i < inputs.Count; i++)
            {
                Assert.AreEqual(doubleService.Run(inputs[i]).Value, control[i].Value);
            }
        }

        [TestMethod]
        public void SmallIntegerServiceRejectsBadInputs()
        {
            var inputs = new List<string>() {"98", "30", "5", "0", "3.5", "-5", "99", "-", "###"};
            var control = new List<Result<Small>>()
            {
                Result.Success<Small>(Small.CreateSmall((uint) 98).Value),
                Result.Success<Small>(Small.CreateSmall((uint) 30).Value),
                Result.Success<Small>(Small.CreateSmall((uint) 5).Value),
                Result.Success<Small>(Small.CreateSmall((uint) 0).Value),
                Result.Failure<Small>("TestError"),
                Result.Failure<Small>("TestError"),
                Result.Failure<Small>("TestError"),
                Result.Failure<Small>("TestError"),
                Result.Failure<Small>("TestError")
            };
            var smallService = new InputServiceSmallInt();
            Result<Small> testResult;

            for (int i = 0; i < inputs.Count; i++)
            {
                testResult = smallService.Run(inputs[i]);
                Assert.AreEqual(testResult.IsSuccess, control[i].IsSuccess);
            }
        }

        [TestMethod]
        public void SmallIntegerServiceValuesCorrect()
        {
            var inputs = new List<string>() {"98", "30", "5", "0"};
            var control = new List<Result<Small>>()
            {
                Result.Success<Small>(Small.CreateSmall((uint) 98).Value),
                Result.Success<Small>(Small.CreateSmall((uint) 30).Value),
                Result.Success<Small>(Small.CreateSmall((uint) 5).Value),
                Result.Success<Small>(Small.CreateSmall((uint) 0).Value)
            };
            var smallService = new InputServiceSmallInt();
            Result<Small> testResult;

            for (int i = 0; i < inputs.Count; i++)
            {
                testResult = smallService.Run(inputs[i]);
                Assert.AreEqual(testResult.Value, control[i].Value);
            }
        }

        [TestMethod]
        public void ProbabilityServiceRejectsBadInputs()
        {
            var inputs = new List<string>() {"1", "0.6", "0.001", "0", "1.002", "-0.001", "-", "###"};
            var control = new List<Result<Probability>>()
            {
                Result.Success<Probability>(Probability.Create(1).Value),
                Result.Success<Probability>(Probability.Create(0.6).Value),
                Result.Success<Probability>(Probability.Create(0.001).Value),
                Result.Success<Probability>(Probability.Create(0).Value),
                Result.Failure<Probability>("TestError"),
                Result.Failure<Probability>("TestError"),
                Result.Failure<Probability>("TestError"),
                Result.Failure<Probability>("TestError")
            };
            var probabilityService = new InputServiceProbability();
            Result<Probability> testResult;

            for (int i = 0; i < inputs.Count; i++)
            {
                testResult = probabilityService.Run(inputs[i]);
                Assert.AreEqual(testResult.IsSuccess, control[i].IsSuccess);
            }
        }

        [TestMethod]
        public void ProbabilityServiceValuesCorrect()
        {
            var inputs = new List<string>() { "1", "0.6", "0.001", "0" };
            var control = new List<Result<Probability>>()
            {
                Result.Success<Probability>(Probability.Create(1).Value),
                Result.Success<Probability>(Probability.Create(0.6).Value),
                Result.Success<Probability>(Probability.Create(0.001).Value),
                Result.Success<Probability>(Probability.Create(0).Value)
            };
            var probabilityService = new InputServiceProbability();
            Result<Probability> testResult;

            for (int i = 0; i < inputs.Count; i++)
            {
                testResult = probabilityService.Run(inputs[i]);
                Assert.AreEqual(testResult.Value, control[i].Value);
            }
        }

    }
}
