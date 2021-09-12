using CSharpFunctionalExtensions;
using ProbabilitySolver.Structs;

namespace ProbabilitySolver.Services.InputService
{
    public class InputServiceSmallInt : IInputService< uint, Small>
    {
        public string _input { get; set; }

        public string _message { get; set; }

        public Result<Small> Run(string input)
        {
            var x = new Result<Small>();
            var f = Parse(input).Tap((k) => x = Convert(k));
            return f.IsSuccess ? x : Result.Failure<Small>(f.Error);
        }

        public Result<uint> Parse(string input)
        {
            uint value;
            var success = uint.TryParse(input, out value);
            return success
                ? Result.Success(value)
                : Result.Failure<uint>($"Cannot convert {input} to a positive integer");
        }

        public Result<Small> Convert(uint input)
        {
            return Small.CreateSmall(input);
        }

    }
}
