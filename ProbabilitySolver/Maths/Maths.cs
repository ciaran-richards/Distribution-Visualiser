namespace ProbabilitySolver.Maths
{
    public static class Maths
    {
        // Returns the value of n! Where the result will not exceed the maximum double value.
     
        public static double Factorial(uint n)
        {
            
            double dref = 1;
            for (uint i = n; i > 0; i--)
            {
                dref *= i;
            }

            return dref;
        }

        public static double BiCoef(uint n, uint p)
        {
            if (n == p || p == 0)
            {
                return 1;
            }
            if (n < p)
            {
                var r = n;
                n = p;
                p = r;
            }

            return Factorial(n) / (Factorial(p) * Factorial(n-p));
        }

    }
}
