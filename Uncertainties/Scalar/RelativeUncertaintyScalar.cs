#if NET8_0_OR_GREATER
using System;
using System.Linq;
using System.Numerics;

namespace VividOrange.Uncertainties.Scalar
{
    public class RelativeUncertaintyScalar<T> : IRelativeUncertainty<T> where T : INumber<T>
    {
        public T CentralValue { get; set; }
        public double RelativeUncertaintyValue { get; set; }
        public T LowerBound
            => CentralValue * T.CreateChecked(1 - RelativeUncertaintyValue);
        public T UpperBound
            => CentralValue * T.CreateChecked(1 + RelativeUncertaintyValue);

        public RelativeUncertaintyScalar(T centralValue, double relativeUncertainty)
        {
            CentralValue = centralValue;
            RelativeUncertaintyValue = relativeUncertainty;
        }

        public IUncertainty<T> PropagateBinary(IUncertainty<T> other, Func<T, T, T> operation)
        {
            if (other is not IRelativeUncertainty<T> rel)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            T[] candidates = new[]
            {
                operation(LowerBound, rel.LowerBound),
                operation(LowerBound, rel.UpperBound),
                operation(UpperBound, rel.LowerBound),
                operation(UpperBound, rel.UpperBound)
            };

            T newCentralValue = operation(CentralValue, rel.CentralValue);
            return new IntervalUncertaintyScalar<T>(
                newCentralValue, candidates.Min(), candidates.Max());
        }

        public IUncertainty<T> PropagateUnary(Func<double, double> operation)
        {
            double newCentral = operation(double.CreateChecked(CentralValue));
            return new RelativeUncertaintyScalar<T>(
                T.CreateChecked(newCentral), RelativeUncertaintyValue);
        }
    }
}
#endif
