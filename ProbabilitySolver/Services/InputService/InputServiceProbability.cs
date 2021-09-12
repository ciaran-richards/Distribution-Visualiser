using CSharpFunctionalExtensions;
using ProbabilitySolver.Structs;

namespace ProbabilitySolver.Services.InputService
{
    public class InputServiceProbability : IInputService< double, Probability>
    {

        public Result<Probability> Run(string input)
        {
            var x = new Result<Probability>();
            var f = Parse(input).Tap((k) => x = Convert(k));
            return f.IsSuccess ? x : Result.Failure<Probability>(f.Error);
        }

        public Result<double> Parse(string input)
        {
            double value;
            var success  = double.TryParse(input, out value);
            return success
                ? Result.Success(value)
                : Result.Failure<double>($"Cannot convert {input} to a decimal number");
        }

        public Result<Probability> Convert(double input)
        {
            return Probability.Create(input);
        }
    }
}
