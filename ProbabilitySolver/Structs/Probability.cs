using CSharpFunctionalExtensions;

namespace ProbabilitySolver.Structs
{
    public struct Probability
    {
        public readonly double _val;

        private Probability(double i)
        {
            if (i <= 1 && i >= 0)
                _val = i;
            else
            {
                _val = 0;
            }
        }
        public static Result<Probability> Create(double i)
        {
            if (i <= 1 && i >= 0)
            {
                return Result.Success(new Probability(i));
            }
            else
            {
                return Result.Failure<Probability>("Probability p must be 0 <= p <= 1 ");
            }
        }
    }

}
