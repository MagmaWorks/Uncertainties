using System;
using System.Linq;

namespace VividOrange.Uncertainties
{
    public class IntervalUncertainty : IIntervalUncertainty<double>
    {
        public double CentralValue { get; set; }
        public double LowerBound { get; set; }
        public double UpperBound { get; set; }

        public IntervalUncertainty(double centralValue, double boundStart, double boundEnd)
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

        public IUncertainty<double> PropagateBinary(IUncertainty<double> other, Func<double, double, double> operation)
        {
            if (other is not IIntervalUncertainty<double> interval)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            double[] candidates = new[]
            {
                operation(LowerBound, interval.LowerBound),
                operation(LowerBound, interval.UpperBound),
                operation(UpperBound, interval.LowerBound),
                operation(UpperBound, interval.UpperBound)
            };

            double newCentralValue = operation(CentralValue, other.CentralValue);
            return new IntervalUncertainty(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<double> PropagateUnary(Func<double, double> operation)
        {
            double low = operation(LowerBound);
            double high = operation(UpperBound);
            double newCentral = operation(CentralValue);
            return new IntervalUncertainty(newCentral, low, high);
        }
    }
}
