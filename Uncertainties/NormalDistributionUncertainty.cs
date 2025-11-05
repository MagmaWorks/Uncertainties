using System;

namespace VividOrange.Uncertainties
{
    public class NormalDistributionUncertainty : INormalDistributionUncertainty<double>
    {
        public double CentralValue { get; set; }
        public double StandardDeviation { get; set; }
        public double LowerBound =>
            CentralValue - (StandardDeviation * CoverageFactor);
        public double UpperBound =>
            CentralValue + (StandardDeviation * CoverageFactor);
        public double CoverageFactor { get; set; } = 3.0;

        public NormalDistributionUncertainty(
            double mean, double standardDeviation, double coverageFactor = 3.0)
        {
            if (standardDeviation < 0)
            {
                throw new ArgumentException("Standard deviation must be a positive number");
            }

            CentralValue = mean;
            StandardDeviation = standardDeviation;
            CoverageFactor = coverageFactor;
        }

        public IUncertainty<double> PropagateBinary(
            IUncertainty<double> other, Func<double, double, double> operation)
        {
            if (other is not INormalDistributionUncertainty<double> norm)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            double central = operation(CentralValue, norm.CentralValue);

            // Variance combines for independent normals
            double variance = Math.Pow(StandardDeviation, 2)
                + Math.Pow(norm.StandardDeviation, 2);
            double sigma = Math.Sqrt(variance);
            return new NormalDistributionUncertainty(central, sigma);
        }

        public IUncertainty<double> PropagateUnary(Func<double, double> operation)
        {
            double newCentral = operation(CentralValue);
            double factor = Math.Abs(operation(1.0)); // scale uncertainty
            double newUncertainty = StandardDeviation * factor;
            return new NormalDistributionUncertainty(
                 newCentral, newUncertainty, CoverageFactor);
        }
    }
}
