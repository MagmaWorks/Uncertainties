using System;
using System.Linq;

namespace VividOrange.Uncertainties
{
    public class RelativeUncertainty : IRelativeUncertainty<double>
    {
        public double CentralValue { get; set; }
        public double RelativeUncertaintyValue { get; set; }
        public double LowerBound
            => CentralValue * (1 - RelativeUncertaintyValue);
        public double UpperBound
            => CentralValue * (1 + RelativeUncertaintyValue);

        public RelativeUncertainty(double centralValue, double relativeUncertainty)
        {
            CentralValue = centralValue;
            RelativeUncertaintyValue = relativeUncertainty;
        }

        public IUncertainty<double> PropagateBinary(IUncertainty<double> other, Func<double, double, double> operation)
        {
            if (other is not IRelativeUncertainty<double> rel)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            double[] candidates = new[]
            {
                operation(LowerBound, rel.LowerBound),
                operation(LowerBound, rel.UpperBound),
                operation(UpperBound, rel.LowerBound),
                operation(UpperBound, rel.UpperBound)
            };

            double newCentralValue = operation(CentralValue, rel.CentralValue);
            return new IntervalUncertainty(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<double> PropagateUnary(Func<double, double> operation)
        {
            double newCentral = operation(CentralValue);
            return new RelativeUncertainty(
                newCentral, RelativeUncertaintyValue);
        }
    }
}
