using System;
using UnitsNet;
using VividOrange.Uncertainties.Quantities.Utility;

namespace VividOrange.Uncertainties.Quantities
{
    public class NormalDistributionUncertaintyQuantity<TQuantity>
        : INormalDistributionUncertainty<TQuantity> where TQuantity : IQuantity
    {
        public TQuantity CentralValue { get; set; }
        public TQuantity StandardDeviation { get; set; }
        public TQuantity LowerBound
            => CentralValue.Subtract(StandardDeviation.Multiply(CoverageFactor));
        public TQuantity UpperBound
            => CentralValue.Add(StandardDeviation.Multiply(CoverageFactor));
        public double CoverageFactor { get; set; } = 3.0;

        public NormalDistributionUncertaintyQuantity(
            TQuantity mean, TQuantity standardDeviation, double coverageFactor = 3.0)
        {
            CentralValue = mean;
            StandardDeviation = standardDeviation;
            CoverageFactor = coverageFactor;
        }

        public IUncertainty<TQuantity> PropagateBinary(
            IUncertainty<TQuantity> other, Func<TQuantity, TQuantity, TQuantity> operation)
        {
            if (other is not INormalDistributionUncertainty<TQuantity> norm)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            TQuantity newCentral = operation(CentralValue, norm.CentralValue);

            // Variance combines for independent normals
            double variance = Math.Pow(StandardDeviation.As(CentralValue.Unit), 2)
                + Math.Pow(norm.StandardDeviation.As(CentralValue.Unit), 2);
            double sigma = Math.Sqrt(variance);
            return new NormalDistributionUncertaintyQuantity<TQuantity>(
                newCentral, CentralValue.WithValue(sigma), CoverageFactor);
        }

        public IUncertainty<TQuantity> PropagateUnary(Func<double, double> operation)
        {
            TQuantity newCentral = CentralValue.WithValue(
                operation(CentralValue.As(CentralValue.Unit)));
            double factor = Math.Abs(operation(1.0)); // scale uncertainty
            TQuantity newUncertainty = StandardDeviation.Multiply(factor);
            return new NormalDistributionUncertaintyQuantity<TQuantity>(
                newCentral, newUncertainty, CoverageFactor);
        }
    }
}
