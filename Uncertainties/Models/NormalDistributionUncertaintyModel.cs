using System;
using System.Linq;

namespace MagmaWorks.Uncertainties.Models
{
    public class NormalDistributionUncertaintyModel : IUncertaintyModel
    {
        public double Mean { get; }
        public double StandardDeviation { get; }
        public double CoverageFactor { get; }

        public NormalDistributionUncertaintyModel(double mean, double stdDev, double coverageFactor = 3.0)
        {
            if (stdDev < 0)
                throw new ArgumentException("Standard deviation must be non-negative.");

            Mean = mean;
            StandardDeviation = stdDev;
            CoverageFactor = coverageFactor;
        }

        public double CentralValue => Mean;
        public double LowerBound => Mean - CoverageFactor * StandardDeviation;
        public double UpperBound => Mean + CoverageFactor * StandardDeviation;

        public IUncertaintyModel PropagateBinary(IUncertaintyModel other, Func<double, double, double> op)
        {
            if (other is not NormalDistributionUncertaintyModel norm)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            var central = op(CentralValue, norm.CentralValue);

            // Variance combines for independent normals
            var variance = Math.Pow(StandardDeviation, 2) + Math.Pow(norm.StandardDeviation, 2);
            var sigma = Math.Sqrt(variance);

            return new NormalDistributionUncertaintyModel(central, sigma);
        }

        public IUncertaintyModel PropagateUnary(Func<double, double> op)
        {
            var newCentral = op(CentralValue);
            var factor = Math.Abs(op(1.0));
            var newSigma = StandardDeviation * factor;
            return new NormalDistributionUncertaintyModel(newCentral, newSigma);
        }
    }
}
