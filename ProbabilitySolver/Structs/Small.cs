using CSharpFunctionalExtensions;

namespace ProbabilitySolver.Structs
{
    // Returns an Integer in the range of 0 - 170 inclusive.
    public struct Small
    {
        public readonly uint _val;

        private Small(uint i)
        {
            if (i < 99)
                _val = i;
            else
            {
                _val = 0;
            }
        }
        public static Result<Small> CreateSmall(uint i)
        {
            if (i < 99)
            {
                return Result.Success(new Small(i));
            }
            else
            {
                return Result.Failure<Small>("Integer i must be 0 <= i <= 99");
            }
        }
    } 
}
