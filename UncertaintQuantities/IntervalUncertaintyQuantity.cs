using System;
using System.Linq;
using UnitsNet;
using VividOrange.Uncertainties.Quantities.Utility;

namespace VividOrange.Uncertainties.Quantities
{
    public class IntervalUncertaintyQuantity<TQuantity>
        : IIntervalUncertainty<TQuantity> where TQuantity : IQuantity
    {
        public TQuantity CentralValue { get; set; }
        public TQuantity LowerBound { get; set; }
        public TQuantity UpperBound { get; set; }

        public IntervalUncertaintyQuantity(
            TQuantity centralValue, TQuantity boundStart, TQuantity boundEnd)
        {
            CentralValue = centralValue;
            if ((double)boundStart.Value > boundEnd.As(boundStart.Unit))
            {
                UpperBound = boundStart;
                LowerBound = boundEnd;
            }
            else
            {
                LowerBound = boundStart;
                UpperBound = boundEnd;
            }
        }

        public IUncertainty<TQuantity> PropagateBinary(
            IUncertainty<TQuantity> other, Func<TQuantity, TQuantity, TQuantity> operation)
        {
            if (other is not IIntervalUncertainty<TQuantity> interval)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            TQuantity[] candidates = new[]
            {
                operation(LowerBound, interval.LowerBound),
                operation(LowerBound, interval.UpperBound),
                operation(UpperBound, interval.LowerBound),
                operation(UpperBound, interval.UpperBound)
            };

            TQuantity newCentralValue = operation(CentralValue, interval.CentralValue);
            return new IntervalUncertaintyQuantity<TQuantity>(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<TQuantity> PropagateUnary(Func<double, double> operation)
        {
            TQuantity low = CentralValue.WithValue(operation(LowerBound.As(CentralValue.Unit)));
            TQuantity high = CentralValue.WithValue(operation(UpperBound.As(CentralValue.Unit)));
            TQuantity newCentral = CentralValue.WithValue(operation((double)CentralValue.Value));
            return new IntervalUncertaintyQuantity<TQuantity>(newCentral, low, high);
        }
    }
}
