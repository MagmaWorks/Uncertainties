#if NET8_0_OR_GREATER
using System;
using System.Linq;
using System.Numerics;

namespace VividOrange.Uncertainties.Scalar
{
    public class IntervalUncertaintyScalar<T> : IIntervalUncertainty<T> where T : INumber<T>
    {
        public T CentralValue { get; set; }
        public T LowerBound { get; set; }
        public T UpperBound { get; set; }

        public IntervalUncertaintyScalar(T centralValue, T boundStart, T boundEnd)
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

        public IUncertainty<T> PropagateBinary(IUncertainty<T> other, Func<T, T, T> operation)
        {
            if (other is not IIntervalUncertainty<T> interval)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            T[] candidates = new[]
            {
                operation(LowerBound, interval.LowerBound),
                operation(LowerBound, interval.UpperBound),
                operation(UpperBound, interval.LowerBound),
                operation(UpperBound, interval.UpperBound)
            };

            T newCentralValue = operation(CentralValue, interval.CentralValue);
            return new IntervalUncertaintyScalar<T>(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<T> PropagateUnary(Func<double, double> operation)
        {
            double low = operation(double.CreateChecked(LowerBound));
            double high = operation(double.CreateChecked(UpperBound));
            double newCentral = operation(double.CreateChecked(CentralValue));
            return new IntervalUncertaintyScalar<T>(
                T.CreateChecked(newCentral), T.CreateChecked(low), T.CreateChecked(high));
        }
    }
}
#endif
