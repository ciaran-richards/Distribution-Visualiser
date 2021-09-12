using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProbabilitySolver.Services.FileService;
using Dasync.Collections;

namespace ProbabilitySolver.Distributions
{
    public struct NormalDistribution : IContinuousDistribution
    {
        public double[] Cases { get; }
        public double[] Probabilities { get; }
        public double[] SumProbability { get; }

        private double coefficientConst;

        private double quotientConst;


        public NormalDistribution(double mean, double stdDev)
        {
            Cases = new double[511];
            Probabilities = new double[511];
            SumProbability = new double[511];
            coefficientConst = 1 / (stdDev * Math.Sqrt(2 * Math.PI));
            quotientConst = 1 / (2 * stdDev * stdDev);
        }


        public async Task WriteData(double mean, double stdDev)
        {
            var ErrorFunction = new ErrorFunctionService();

            var errorData = await ErrorFunction.LoadCases();
            var keys = errorData.Item1;
            var values = errorData.Item2;

            this.Cases[255] = mean;
            this.SumProbability[255] = values[0];
            for (int i = 0; i <= 255; i++)
            {
                this.Cases[255 + i] = mean + (stdDev * keys[i]);
                this.Cases[255 - i] = mean - (stdDev * keys[i]);

                this.Probabilities[255 - i] = Density(Cases[255-i], mean);
                this.Probabilities[255 + i] = Probabilities[255 - i];

                this.SumProbability[255 + i] = values[i];
                this.SumProbability[255 - i] = 1 - values[i];
            }
        }

        double Density(double x, double mean)
        {
            var exponent = - (x - mean) * (x - mean) * quotientConst;
            var density = coefficientConst * Math.Exp(exponent);
            return density;
        }
    }
}
