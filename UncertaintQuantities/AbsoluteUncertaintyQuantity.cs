using System;
using System.Linq;
using MagmaWorks.Uncertainties.Quantities.Utility;
using UnitsNet;

namespace MagmaWorks.Uncertainties.Quantities
{
    public class AbsoluteUncertaintyQuantity<TQuantity>
        : IAbsoluteUncertainty<TQuantity> where TQuantity : IQuantity
    {
        public TQuantity AbsoluteUncertaintyValue { get; set; }
        public TQuantity CentralValue { get; set; }
        public TQuantity LowerBound => CentralValue.Subtract(AbsoluteUncertaintyValue);
        public TQuantity UpperBound => CentralValue.Add(AbsoluteUncertaintyValue);

        public AbsoluteUncertaintyQuantity(TQuantity centralValue, TQuantity uncertainty)
        {
            CentralValue = centralValue;
            AbsoluteUncertaintyValue = uncertainty;
        }

        public IUncertainty<TQuantity> PropagateBinary(
            IUncertainty<TQuantity> other, Func<TQuantity, TQuantity, TQuantity> operation)
        {
            if (other is not IAbsoluteUncertainty<TQuantity> abs)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            TQuantity[] candidates = new[]
            {
                operation(LowerBound, abs.LowerBound),
                operation(LowerBound, abs.UpperBound),
                operation(UpperBound, abs.LowerBound),
                operation(UpperBound, abs.UpperBound)
            };

            TQuantity newCentralValue = operation(CentralValue, abs.CentralValue);
            return new IntervalUncertaintyQuantity<TQuantity>(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<TQuantity> PropagateUnary(Func<double, double> operation)
        {
            double newCentral = operation(CentralValue.As(CentralValue.Unit));
            double factor = Math.Abs(operation(1.0)); // scale uncertainty
            TQuantity newUncertainty = AbsoluteUncertaintyValue.Multiply(factor);
            return new AbsoluteUncertaintyQuantity<TQuantity>(
                CentralValue.WithValue(newCentral), newUncertainty);
        }
    }
}
