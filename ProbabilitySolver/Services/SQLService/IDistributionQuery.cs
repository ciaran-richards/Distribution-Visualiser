using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProbabilitySolver.Structs;

namespace ProbabilitySolver.Services.SQLService
{
    public interface IDistributionQuery
    {
        Result<List<DistributionRow>> GetAllDistributions();
    }
}
