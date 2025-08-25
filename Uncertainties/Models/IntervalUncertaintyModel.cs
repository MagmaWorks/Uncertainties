using System;
using System.Linq;

namespace MagmaWorks.Uncertainties.Models
{
    public class IntervalUncertaintyModel : IUncertaintyModel
    {
        public double LowerBound { get; }
        public double UpperBound { get; }

        public IntervalUncertaintyModel(double lower, double upper)
        {
            if (upper < lower)
                throw new ArgumentException("Upper bound must be >= lower bound.");

            LowerBound = lower;
            UpperBound = upper;
        }

        public IUncertaintyModel PropagateBinary(IUncertaintyModel other, Func<double, double, double> op)
        {
            if (other is not IntervalUncertaintyModel interval)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            var candidates = new[]
            {
                op(LowerBound, interval.LowerBound),
                op(LowerBound, interval.UpperBound),
                op(UpperBound, interval.LowerBound),
                op(UpperBound, interval.UpperBound)
            };

            return new IntervalUncertaintyModel(candidates.Min(), candidates.Max());
        }

        public IUncertaintyModel PropagateUnary(Func<double, double> op)
        {
            var low = op(LowerBound);
            var high = op(UpperBound);
            return new IntervalUncertaintyModel(Math.Min(low, high), Math.Max(low, high));
        }
    }
}
