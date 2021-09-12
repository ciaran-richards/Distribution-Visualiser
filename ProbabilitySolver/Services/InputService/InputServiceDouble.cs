using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace ProbabilitySolver.Services.InputService
{
    public class InputServiceDouble : IInputService<double, double>
    {
        public Result<double> Run(string input)
        {
            return Parse(input).Tap((x) => Convert(x));
        }

        public Result<double> Parse(string input)
        {
            try
            {
                return Result.Success(double.Parse(input));
            }
            catch (Exception)
            {
                return Result.Failure<double>($"Cannot convert {input} to decimal number");
            }
        }

        public Result<double> Convert(double input)
        {
            return Result.Success(input);
        }
    }
}
