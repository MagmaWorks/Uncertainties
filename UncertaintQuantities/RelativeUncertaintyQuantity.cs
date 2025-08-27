using System;
using System.Linq;
using MagmaWorks.Uncertainties.Quantities.Utility;
using UnitsNet;

namespace MagmaWorks.Uncertainties.Quantities
{
    public class RelativeUncertaintyQuantity<TQuantity>
        : IRelativeUncertainty<TQuantity> where TQuantity : IQuantity
    {
        public TQuantity CentralValue { get; set; }
        public TQuantity LowerBound
            => CentralValue.Multiply(1 - RelativeUncertaintyValue);
        public TQuantity UpperBound
            => CentralValue.Multiply(1 + RelativeUncertaintyValue);
        public double RelativeUncertaintyValue { get; set; }

        public RelativeUncertaintyQuantity(TQuantity centralValue, double uncertainty)
        {
            CentralValue = centralValue;
            RelativeUncertaintyValue = uncertainty;
        }

        public RelativeUncertaintyQuantity(TQuantity centralValue, Ratio uncertainty)
            : this(centralValue, uncertainty.DecimalFractions) { }

        public IUncertainty<TQuantity> PropagateBinary(
            IUncertainty<TQuantity> other, Func<TQuantity, TQuantity, TQuantity> operation)
        {
            if (other is not IRelativeUncertainty<TQuantity> rel)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            TQuantity[] candidates = new[]
            {
                operation(LowerBound, rel.LowerBound),
                operation(LowerBound, rel.UpperBound),
                operation(UpperBound, rel.LowerBound),
                operation(UpperBound, rel.UpperBound)
            };

            TQuantity newCentralValue = operation(CentralValue, other.CentralValue);
            return new IntervalUncertaintyQuantity<TQuantity>(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<TQuantity> PropagateUnary(Func<double, double> operation)
        {
            double newCentral = operation(CentralValue.As(CentralValue.Unit));
            return new RelativeUncertaintyQuantity<TQuantity>(
                CentralValue.WithValue(newCentral), RelativeUncertaintyValue);
        }
    }
}
