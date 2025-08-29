using System;

namespace MagmaWorks.Uncertainties.Decimal
{
    public class NormalDistributionUncertaintyDecimal : INormalDistributionUncertainty<decimal>
    {
        public decimal CentralValue { get; set; }
        public decimal StandardDeviation { get; set; }
        public decimal LowerBound =>
            CentralValue - (StandardDeviation * (decimal)CoverageFactor);
        public decimal UpperBound =>
            CentralValue + (StandardDeviation * (decimal)CoverageFactor);
        public double CoverageFactor { get; set; } = 3.0;

        public NormalDistributionUncertaintyDecimal(
            decimal mean, decimal standardDeviation, double coverageFactor = 3.0)
        {
            if (standardDeviation < 0)
            {
                throw new ArgumentException("Standard deviation must be a positive number");
            }

            CentralValue = mean;
            StandardDeviation = standardDeviation;
            CoverageFactor = coverageFactor;
        }

        public IUncertainty<decimal> PropagateBinary(
            IUncertainty<decimal> other, Func<decimal, decimal, decimal> operation)
        {
            if (other is not INormalDistributionUncertainty<decimal> norm)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            decimal central = operation(CentralValue, norm.CentralValue);

            // Variance combines for independent normals
            double variance = Math.Pow((double)StandardDeviation, 2)
                + Math.Pow((double)norm.StandardDeviation, 2);
            double sigma = Math.Sqrt(variance);
            return new NormalDistributionUncertaintyDecimal(central, (decimal)sigma);
        }

        public IUncertainty<decimal> PropagateUnary(Func<double, double> operation)
        {
            double newCentral = operation((double)CentralValue);
            double factor = Math.Abs(operation(1.0)); // scale uncertainty
            double newUncertainty = (double)StandardDeviation * factor;
            return new NormalDistributionUncertaintyDecimal(
                 (decimal)newCentral, (decimal)newUncertainty, CoverageFactor);
        }
    }
}
