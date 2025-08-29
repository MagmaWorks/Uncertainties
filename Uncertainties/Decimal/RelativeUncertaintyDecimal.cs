using System;
using System.Linq;

namespace MagmaWorks.Uncertainties.Decimal
{
    public class RelativeUncertaintyDecimal : IRelativeUncertainty<decimal>
    {
        public decimal CentralValue { get; set; }
        public double RelativeUncertaintyValue { get; set; }
        public decimal LowerBound
            => CentralValue * (1 - (decimal)RelativeUncertaintyValue);
        public decimal UpperBound
            => CentralValue * (1 + (decimal)RelativeUncertaintyValue);

        public RelativeUncertaintyDecimal(decimal centralValue, double relativeUncertainty)
        {
            CentralValue = centralValue;
            RelativeUncertaintyValue = relativeUncertainty;
        }

        public IUncertainty<decimal> PropagateBinary(IUncertainty<decimal> other, Func<decimal, decimal, decimal> operation)
        {
            if (other is not IRelativeUncertainty<decimal> rel)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            decimal[] candidates = new[]
            {
                operation(LowerBound, rel.LowerBound),
                operation(LowerBound, rel.UpperBound),
                operation(UpperBound, rel.LowerBound),
                operation(UpperBound, rel.UpperBound)
            };

            decimal newCentralValue = operation(CentralValue, rel.CentralValue);
            return new IntervalUncertaintyDecimal(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<decimal> PropagateUnary(Func<double, double> operation)
        {
            double newCentral = operation((double)CentralValue);
            return new RelativeUncertaintyDecimal(
                (decimal)newCentral, RelativeUncertaintyValue);
        }
    }
}
