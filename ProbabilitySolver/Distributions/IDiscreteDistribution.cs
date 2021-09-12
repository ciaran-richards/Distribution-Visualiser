using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProbabilitySolver.Distributions;

namespace ProbabilitySolver
{
    public interface IDiscreteDistribution : IDistribution
    {
        uint[] Cases { get; }

    }
}
