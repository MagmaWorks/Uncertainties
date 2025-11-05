using System;
using System.Linq;

namespace VividOrange.Uncertainties.Decimal
{
    public class AbsoluteUncertaintyDecimal : IAbsoluteUncertainty<decimal>
    {
        public decimal AbsoluteUncertaintyValue { get; set; }
        public decimal CentralValue { get; set; }
        public decimal LowerBound => CentralValue - AbsoluteUncertaintyValue;
        public decimal UpperBound => CentralValue + AbsoluteUncertaintyValue;

        public AbsoluteUncertaintyDecimal(decimal centralValue, decimal uncertainty)
        {
            CentralValue = centralValue;
            AbsoluteUncertaintyValue = uncertainty;
        }

        public IUncertainty<decimal> PropagateBinary(
            IUncertainty<decimal> other, Func<decimal, decimal, decimal> operation)
        {
            if (other is not IAbsoluteUncertainty<decimal> abs)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            decimal[] candidates = new[]
            {
                operation(LowerBound, abs.LowerBound),
                operation(LowerBound, abs.UpperBound),
                operation(UpperBound, abs.LowerBound),
                operation(UpperBound, abs.UpperBound)
            };

            decimal newCentralValue = operation(CentralValue, abs.CentralValue);
            return new IntervalUncertaintyDecimal(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<decimal> PropagateUnary(Func<double, double> operation)
        {
            double newCentral = operation((double)CentralValue);
            double factor = Math.Abs(operation(1.0)); // scale uncertainty
            decimal newUncertainty = AbsoluteUncertaintyValue * (decimal)factor;
            return new AbsoluteUncertaintyDecimal((decimal)newCentral, newUncertainty);
        }
    }
}
