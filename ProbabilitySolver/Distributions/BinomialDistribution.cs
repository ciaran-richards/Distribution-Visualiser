using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProbabilitySolver.Structs;

namespace ProbabilitySolver
{
    public struct BinomialDistribution : IDiscreteDistribution
    {
        public uint _size { get; }
        public uint[] Cases { get; }
        public double[] Probabilities { get; }
        public double[] SumProbability { get; }

        public BinomialDistribution(Small size, Probability probability)
        {
            if (size._val == 0)
            {
                _size = 0;
                Cases = new uint[] {0};
                Probabilities = new double[] {1};
            }
            
            double p = probability._val;
            uint n = size._val;
            this._size = size._val;
            this.Cases = new uint[n+1];
            this.Probabilities = new double[n+1];
            this.SumProbability = new double[n+1];
            for (uint i = 0 ; i <= n; i++)
            {
                Cases[i] = i;
                var nCr = Maths.Maths.BiCoef(n, Cases[i]);
                this.Probabilities[i] = Math.Pow(p, i) * Math.Pow((1 - p), (n - i)) * nCr;
            }

            for (uint i = 0; i <= n; i++)
            {
                if (i!=0) 
                    this.SumProbability[i] = SumProbability[i-1] + Probabilities[i];
                else
                {
                    this.SumProbability[0] = Probabilities[0];
                }
            }
        }
    }
}
