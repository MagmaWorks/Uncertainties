#if NET8_0_OR_GREATER
using System;
using System.Numerics;

namespace VividOrange.Uncertainties.Scalar
{
    public class NormalDistributionUncertaintyScalar<T>
        : INormalDistributionUncertainty<T> where T : INumber<T>
    {
        public T CentralValue { get; set; }
        public T StandardDeviation { get; set; }
        public T LowerBound =>
            CentralValue - (StandardDeviation * T.CreateChecked(CoverageFactor));
        public T UpperBound =>
            CentralValue + (StandardDeviation * T.CreateChecked(CoverageFactor));
        public double CoverageFactor { get; set; } = 3.0;

        public NormalDistributionUncertaintyScalar(
            T mean, T standardDeviation, double coverageFactor = 3.0)
        {
            if (standardDeviation < T.Zero)
            {
                throw new ArgumentException("Standard deviation must be a positive number");
            }

            CentralValue = mean;
            StandardDeviation = standardDeviation;
            CoverageFactor = coverageFactor;
        }

        public IUncertainty<T> PropagateBinary(IUncertainty<T> other, Func<T, T, T> operation)
        {
            if (other is not INormalDistributionUncertainty<T> norm)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            T central = operation(CentralValue, norm.CentralValue);

            // Variance combines for independent normals
            double variance = Math.Pow(double.CreateChecked(StandardDeviation), 2)
                + Math.Pow(double.CreateChecked(norm.StandardDeviation), 2);
            double sigma = Math.Sqrt(variance);
            return new NormalDistributionUncertaintyScalar<T>(central, T.CreateChecked(sigma));
        }

        public IUncertainty<T> PropagateUnary(Func<double, double> operation)
        {
            double newCentral = operation(double.CreateChecked(CentralValue));
            double factor = Math.Abs(operation(1.0)); // scale uncertainty
            double newUncertainty = double.CreateChecked(StandardDeviation) * factor;
            return new NormalDistributionUncertaintyScalar<T>(
                 T.CreateChecked(newCentral), T.CreateChecked(newUncertainty), CoverageFactor);
        }
    }
}
#endif
