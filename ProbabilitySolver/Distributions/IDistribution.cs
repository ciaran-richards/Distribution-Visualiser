using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilitySolver.Distributions
{
    public interface IDistribution
    {
        double[] Probabilities { get; }
        double[] SumProbability { get; }

    }
}
