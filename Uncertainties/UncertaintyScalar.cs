#if NET8_0_OR_GREATER
using System;
using System.Numerics;
using MagmaWorks.Uncertainties.Scalar;

namespace MagmaWorks.Uncertainties
{
    public class UncertaintyScalar<T> : IUncertainty<T> where T : INumber<T>
    {
        public T CentralValue => UncertaintyModel.CentralValue;
        public T LowerBound => UncertaintyModel.LowerBound;
        public T UpperBound => UncertaintyModel.UpperBound;

        public IUncertainty<T> UncertaintyModel { get; set; }

        /// <summary>
        /// Absolulte uncertainty
        /// </summary>
        /// <param name="centralValue"></param>
        /// <param name="absoluteUncertainty"></param>
        public UncertaintyScalar(T centralValue, T absoluteUncertainty)
        {
            UncertaintyModel =
                new AbsoluteUncertaintyScalar<T>(centralValue, absoluteUncertainty);
        }

        /// <summary>
        /// Relative uncertainty
        /// </summary>
        /// <param name="centralValue"></param>
        /// <param name="relativeUncertainty"></param>
        public UncertaintyScalar(T centralValue, double relativeUncertainty)
        {
            UncertaintyModel =
                new RelativeUncertaintyScalar<T>(centralValue, relativeUncertainty);
        }

        /// <summary>
        /// Interval uncertainty
        /// </summary>
        /// <param name="centralValue"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        public UncertaintyScalar(
            T centralValue, T lowerBound, T upperBound)
        {
            UncertaintyModel = new IntervalUncertaintyScalar<T>(
                centralValue, lowerBound, upperBound);
        }

        /// <summary>
        /// Normal distribution uncertainty
        /// </summary>
        /// <param name="centralValue"></param>
        /// <param name="standardDeviation"></param>
        /// <param name="coverageFactor"></param>
        public UncertaintyScalar(
            T centralValue, T standardDeviation, double coverageFactor = 3.0)
        {
            UncertaintyModel = new NormalDistributionUncertaintyScalar<T>(
                centralValue, standardDeviation, coverageFactor);
        }

        public static UncertaintyScalar<T> FromAbsoluteUncertainty(
            T centralValue, T absoluteUncertainty)
        {
            return new UncertaintyScalar<T>(centralValue, absoluteUncertainty);
        }

        public static UncertaintyScalar<T> FromRelativeUncertainty(
            T centralValue, double relativeUncertainty)
        {
            return new UncertaintyScalar<T>(centralValue, relativeUncertainty);
        }

        public static UncertaintyScalar<T> FromIntervalUncertainty(
            T centralValue, T lowerBound, T upperBound)
        {
            return new UncertaintyScalar<T>(centralValue, lowerBound, upperBound);
        }

        public static UncertaintyScalar<T> FromNormalDistributionUncertainty(
            T centralValue, T standardDeviation, double coverageFactor = 3.0)
        {
            return new UncertaintyScalar<T>(
                centralValue, standardDeviation, coverageFactor);
        }

        public IUncertainty<T> PropagateBinary
            (IUncertainty<T> other, Func<T, T, T> operation) =>
            UncertaintyModel.PropagateBinary(other, operation);
        public IUncertainty<T> PropagateUnary(Func<double, double> operation) =>
            UncertaintyModel.PropagateUnary(operation);
    }
}
#endif
