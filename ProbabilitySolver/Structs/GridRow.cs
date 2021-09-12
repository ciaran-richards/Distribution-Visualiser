using System;


namespace ProbabilitySolver.Structs
{
    public struct GridRow
    {
        public double cases { get; set; }
        public double probability{ get; set; }
        public double sumProbability { get; set; }

        public GridRow(double a, double b, double c)
        {
            cases = (double)a;

            probability = b;
            sumProbability = c;
        }


    }
}
