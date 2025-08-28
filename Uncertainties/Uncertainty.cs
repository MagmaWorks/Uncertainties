using System;
using MagmaWorks.Uncertainties.Utility;
using Model = MagmaWorks.Uncertainties.UncertaintyModel;

namespace MagmaWorks.Uncertainties
{
    public class Uncertainty : IUncertainty<double>
    {
        public double CentralValue => UncertaintyModel.CentralValue;
        public double LowerBound => UncertaintyModel.LowerBound;
        public double UpperBound => UncertaintyModel.UpperBound;

        public IUncertainty<double> UncertaintyModel { get; set; }

        public Uncertainty(IUncertainty<double> model)
        {
            UncertaintyModel = model;
        }

        public Uncertainty(double centralValue, Model model)
        {
            switch (model)
            {
                case Model.Interval:
                    UncertaintyModel = new IntervalUncertainty(centralValue, 0, 0);
                    break;
                case Model.Relative:
                    UncertaintyModel = new RelativeUncertainty(centralValue, 0);
                    break;
                case Model.Absolute:
                    UncertaintyModel = new AbsoluteUncertainty(centralValue, 0);
                    break;
                case Model.NormalDistribution:
                    UncertaintyModel = new NormalDistributionUncertainty(centralValue, 0, 0);
                    break;
            }
        }

        public static Uncertainty operator +(Uncertainty left, Uncertainty right)
            => left.Add(right);

        public static Uncertainty operator -(Uncertainty left, Uncertainty right)
            => left.Subtract(right);

        public static Uncertainty operator *(Uncertainty left, double right)
            => left.Multiply(right);

        public static Uncertainty operator /(Uncertainty left, double right)
        {
            if (right == 0)
            {
                throw new DivideByZeroException();
            }

            return left.Divide(right);
        }

        public static implicit operator Uncertainty(IntervalUncertainty uncertainty)
            => new Uncertainty(uncertainty);

        public static implicit operator Uncertainty(RelativeUncertainty uncertainty)
            => new Uncertainty(uncertainty);

        public static implicit operator Uncertainty(AbsoluteUncertainty uncertainty)
            => new Uncertainty(uncertainty);

        public static implicit operator Uncertainty(NormalDistributionUncertainty uncertainty)
            => new Uncertainty(uncertainty);

        public static explicit operator IntervalUncertainty(Uncertainty uncertainty)
        {
            if (uncertainty != null && uncertainty.UncertaintyModel is IntervalUncertainty interv)
            {
                return interv;
            }

            throw new Exception($"Invalid uncertainty model");
        }

        public static explicit operator RelativeUncertainty(Uncertainty uncertainty)
        {
            if (uncertainty != null && uncertainty.UncertaintyModel is RelativeUncertainty rel)
            {
                return rel;
            }

            throw new Exception($"Invalid uncertainty model");
        }

        public static explicit operator AbsoluteUncertainty(Uncertainty uncertainty)
        {
            if (uncertainty != null && uncertainty.UncertaintyModel is AbsoluteUncertainty abs)
            {
                return abs;
            }

            throw new Exception($"Invalid uncertainty model");
        }

        public static explicit operator NormalDistributionUncertainty(Uncertainty uncertainty)
        {
            if (uncertainty != null &&
                uncertainty.UncertaintyModel is NormalDistributionUncertainty norm)
            {
                return norm;
            }

            throw new Exception($"Invalid uncertainty model");
        }

        public static Uncertainty FromAbsoluteUncertainty(
            double centralValue, double absoluteUncertainty)
        {
            return new Uncertainty(
                new AbsoluteUncertainty(centralValue, absoluteUncertainty));
        }

        public static Uncertainty FromRelativeUncertainty(
            double centralValue, double relativeUncertainty)
        {
            return new Uncertainty(
                new RelativeUncertainty(centralValue, relativeUncertainty));
        }

        public static Uncertainty FromIntervalUncertainty(
            double centralValue, double lowerBound, double upperBound)
        {
            return new Uncertainty(
                new IntervalUncertainty(centralValue, lowerBound, upperBound));
        }

        public static Uncertainty FromNormalDistributionUncertainty(
            double centralValue, double standardDeviation, double coverageFactor = 3.0)
        {
            return new Uncertainty(new NormalDistributionUncertainty(
                centralValue, standardDeviation, coverageFactor));
        }

        public IUncertainty<double> PropagateBinary(
            IUncertainty<double> other, Func<double, double, double> operation)
            => UncertaintyModel.PropagateBinary(other, operation);
        public IUncertainty<double> PropagateUnary(Func<double, double> operation)
            => UncertaintyModel.PropagateUnary(operation);
    }
}
