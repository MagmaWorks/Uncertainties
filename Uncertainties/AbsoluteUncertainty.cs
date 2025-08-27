using System;
using System.Linq;

namespace MagmaWorks.Uncertainties
{
    public class AbsoluteUncertainty : IAbsoluteUncertainty<double>
    {
        public double AbsoluteUncertaintyValue { get; set; }
        public double CentralValue { get; set; }
        public double LowerBound => CentralValue - AbsoluteUncertaintyValue;
        public double UpperBound => CentralValue + AbsoluteUncertaintyValue;

        public AbsoluteUncertainty(double centralValue, double uncertainty)
        {
            CentralValue = centralValue;
            AbsoluteUncertaintyValue = uncertainty;
        }

        public IUncertainty<double> PropagateBinary(
            IUncertainty<double> other, Func<double, double, double> operation)
        {
            if (other is not IAbsoluteUncertainty<double> abs)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            double[] candidates = new[]
            {
                operation(LowerBound, abs.LowerBound),
                operation(LowerBound, abs.UpperBound),
                operation(UpperBound, abs.LowerBound),
                operation(UpperBound, abs.UpperBound)
            };

            double newCentralValue = operation(CentralValue, abs.CentralValue);
            return new IntervalUncertainty(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<double> PropagateUnary(Func<double, double> operation)
        {
            double newCentral = operation(CentralValue);
            double factor = Math.Abs(operation(1.0)); // scale uncertainty
            double newUncertainty = AbsoluteUncertaintyValue * factor;
            return new AbsoluteUncertainty(newCentral, newUncertainty);
        }
    }
}
