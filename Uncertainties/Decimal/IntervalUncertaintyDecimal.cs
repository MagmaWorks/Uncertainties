using System;
using System.Linq;

namespace MagmaWorks.Uncertainties.Decimal
{
    public class IntervalUncertaintyDecimal: IIntervalUncertainty<decimal>
    {
        public decimal CentralValue { get; set; }
        public decimal LowerBound { get; set; }
        public decimal UpperBound { get; set; }

        public IntervalUncertaintyDecimal(
            decimal centralValue, decimal boundStart, decimal boundEnd)
        {
            CentralValue = centralValue;
            if (boundStart > boundEnd)
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

        public IUncertainty<decimal> PropagateBinary(
            IUncertainty<decimal> other, Func<decimal, decimal, decimal> operation)
        {
            if (other is not IIntervalUncertainty<decimal> interval)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            decimal[] candidates = new[]
            {
                operation(LowerBound, interval.LowerBound),
                operation(LowerBound, interval.UpperBound),
                operation(UpperBound, interval.LowerBound),
                operation(UpperBound, interval.UpperBound)
            };

            decimal newCentralValue = operation(CentralValue, other.CentralValue);
            return new IntervalUncertaintyDecimal(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<decimal> PropagateUnary(Func<double, double> operation)
        {
            double low = operation((double)LowerBound);
            double high = operation((double)UpperBound);
            double newCentral = operation((double)CentralValue);
            return new IntervalUncertaintyDecimal(
                (decimal)newCentral, (decimal)low, (decimal)high);
        }
    }
}
