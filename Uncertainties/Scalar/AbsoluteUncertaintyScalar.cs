#if NET8_0_OR_GREATER
using System;
using System.Linq;
using System.Numerics;

namespace VividOrange.Uncertainties.Scalar
{
    public class AbsoluteUncertaintyScalar<T> : IAbsoluteUncertainty<T> where T : INumber<T>
    {
        public T AbsoluteUncertaintyValue { get; set; }
        public T CentralValue { get; set; }
        public T LowerBound => CentralValue - AbsoluteUncertaintyValue;
        public T UpperBound => CentralValue + AbsoluteUncertaintyValue;

        public AbsoluteUncertaintyScalar(T centralValue, T uncertainty)
        {
            CentralValue = centralValue;
            AbsoluteUncertaintyValue = uncertainty;
        }


        public IUncertainty<T> PropagateBinary(IUncertainty<T> other, Func<T, T, T> operation)
        {
            if (other is not IAbsoluteUncertainty<T> abs)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            T[] candidates = new[]
            {
                operation(LowerBound, abs.LowerBound),
                operation(LowerBound, abs.UpperBound),
                operation(UpperBound, abs.LowerBound),
                operation(UpperBound, abs.UpperBound)
            };

            T newCentralValue = operation(CentralValue, abs.CentralValue);
            return new IntervalUncertaintyScalar<T>(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<T> PropagateUnary(Func<double, double> operation)
        {
            double newCentral = operation(double.CreateChecked(CentralValue));
            double factor = Math.Abs(operation(1.0)); // scale uncertainty
            double newUncertainty = double.CreateChecked(AbsoluteUncertaintyValue) * factor;
            return new AbsoluteUncertaintyScalar<T>(
                T.CreateChecked(newCentral), T.CreateChecked(newUncertainty));
        }
    }
}
#endif
