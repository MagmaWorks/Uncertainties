using System;
using MagmaWorks.Uncertainties.Quantities;
using UnitsNet;

namespace MagmaWorks.Uncertainties
{
    public class UncertaintyQuantity<T>
        : IUncertainty<T> where T : IQuantity
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
        public UncertaintyQuantity(T centralValue, T absoluteUncertainty)
        {
            UncertaintyModel =
                new AbsoluteUncertaintyQuantity<T>(centralValue, absoluteUncertainty);
        }

        /// <summary>
        /// Relative uncertainty
        /// </summary>
        /// <param name="centralValue"></param>
        /// <param name="relativeUncertainty"></param>
        public UncertaintyQuantity(T centralValue, Ratio relativeUncertainty)
        {
            UncertaintyModel =
                new RelativeUncertaintyQuantity<T>(centralValue, relativeUncertainty);
        }

        /// <summary>
        /// Relative uncertainty
        /// </summary>
        /// <param name="centralValue"></param>
        /// <param name="relativeUncertainty"></param>
        public UncertaintyQuantity(T centralValue, double relativeUncertainty)
        {
            UncertaintyModel =
                new RelativeUncertaintyQuantity<T>(centralValue, relativeUncertainty);
        }

        /// <summary>
        /// Interval uncertainty
        /// </summary>
        /// <param name="centralValue"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        public UncertaintyQuantity(
            T centralValue, T lowerBound, T upperBound)
        {
            UncertaintyModel = new IntervalUncertaintyQuantity<T>(
                centralValue, lowerBound, upperBound);
        }

        /// <summary>
        /// Normal distribution uncertainty
        /// </summary>
        /// <param name="centralValue"></param>
        /// <param name="standardDeviation"></param>
        /// <param name="coverageFactor"></param>
        public UncertaintyQuantity(
            T centralValue, T standardDeviation, double coverageFactor = 3.0)
        {
            UncertaintyModel = new NormalDistributionUncertaintyQuantity<T>(
                centralValue, standardDeviation, coverageFactor);
        }


        internal UncertaintyQuantity(IUncertainty<T> model)
        {
            UncertaintyModel = model;
        }

        public static implicit operator UncertaintyQuantity<T>(
            IntervalUncertaintyQuantity<T> uncertainty) => new UncertaintyQuantity<T>(uncertainty);

        public static implicit operator UncertaintyQuantity<T>(
            RelativeUncertaintyQuantity<T> uncertainty) => new UncertaintyQuantity<T>(uncertainty);

        public static implicit operator UncertaintyQuantity<T>(
            AbsoluteUncertaintyQuantity<T> uncertainty) => new UncertaintyQuantity<T>(uncertainty);

        public static implicit operator UncertaintyQuantity<T>(
            NormalDistributionUncertaintyQuantity<T> uncertainty)
            => new UncertaintyQuantity<T>(uncertainty);

        public static explicit operator IntervalUncertaintyQuantity<T>(
            UncertaintyQuantity<T> uncertainty)
        {
            if (uncertainty != null &&
                uncertainty.UncertaintyModel is IntervalUncertaintyQuantity<T> interv)
            {
                return interv;
            }

            throw new Exception($"Invalid uncertainty model");
        }

        public static explicit operator RelativeUncertaintyQuantity<T>(
            UncertaintyQuantity<T> uncertainty)
        {
            if (uncertainty != null &&
                uncertainty.UncertaintyModel is RelativeUncertaintyQuantity<T> rel)
            {
                return rel;
            }

            throw new Exception($"Invalid uncertainty model");
        }

        public static explicit operator AbsoluteUncertaintyQuantity<T>(
            UncertaintyQuantity<T> uncertainty)
        {
            if (uncertainty != null &&
                uncertainty.UncertaintyModel is AbsoluteUncertaintyQuantity<T> abs)
            {
                return abs;
            }

            throw new Exception($"Invalid uncertainty model");
        }

        public static explicit operator NormalDistributionUncertaintyQuantity<T>(
            UncertaintyQuantity<T> uncertainty)
        {
            if (uncertainty != null &&
                uncertainty.UncertaintyModel is NormalDistributionUncertaintyQuantity<T> norm)
            {
                return norm;
            }

            throw new Exception($"Invalid uncertainty model");
        }

        public static UncertaintyQuantity<T> FromAbsoluteUncertainty(
            T centralValue, T absoluteUncertainty)
        {
            return new UncertaintyQuantity<T>(centralValue, absoluteUncertainty);
        }

        public static UncertaintyQuantity<T> FromRelativeUncertainty(
            T centralValue, Ratio relativeUncertainty)
        {
            return new UncertaintyQuantity<T>(centralValue, relativeUncertainty);
        }

        public static UncertaintyQuantity<T> FromRelativeUncertainty(
            T centralValue, double relativeUncertainty)
        {
            return new UncertaintyQuantity<T>(centralValue, relativeUncertainty);
        }

        public static UncertaintyQuantity<T> FromIntervalUncertainty(
            T centralValue, T lowerBound, T upperBound)
        {
            return new UncertaintyQuantity<T>(centralValue, lowerBound, upperBound);
        }

        public static UncertaintyQuantity<T> FromNormalDistributionUncertainty(
            T centralValue, T standardDeviation, double coverageFactor = 3.0)
        {
            return new UncertaintyQuantity<T>(
                centralValue, standardDeviation, coverageFactor);
        }

        public IUncertainty<T> PropagateBinary
            (IUncertainty<T> other, Func<T, T, T> operation)
            => UncertaintyModel.PropagateBinary(other, operation);
        public IUncertainty<T> PropagateUnary(Func<double, double> operation)
            => UncertaintyModel.PropagateUnary(operation);
    }
}
