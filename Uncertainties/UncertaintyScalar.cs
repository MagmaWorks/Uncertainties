#if NET8_0_OR_GREATER
using System;
using MagmaWorks.Uncertainties.Models;
using UnitsNet;

namespace MagmaWorks.Uncertainties
{
    public class UncertaintyScalar : UncertaintyScalar<double>
    {
        public UncertaintyScalar(double centralValue, IUncertaintyModel model) : base(centralValue, model)
        {
        }

        public static UncertaintyScalar FromAbsoluteUncertainty(
            double centralValue, double absoluteUncertainty)
        {
            return new UncertaintyScalar(
                centralValue,
                new AbsoluteUncertaintyModel(Convert.ToDouble(centralValue), absoluteUncertainty));
        }

        public static UncertaintyScalar FromRelativeUncertainty(
            double centralValue, double relativeUncertainty)
        {
            return new UncertaintyScalar(
                centralValue,
                new RelativeUncertaintyModel(Convert.ToDouble(centralValue), relativeUncertainty));
        }

        public static UncertaintyScalar FromIntervalUncertainty(
            double centralValue, double lowerBound, double upperBound)
        {
            return new UncertaintyScalar(
                centralValue,
                new IntervalUncertaintyModel(lowerBound, upperBound));
        }

        public static UncertaintyScalar FromNormalDistributionUncertainty(
            double centralValue, double standardDeviation, double coverageFactor = 3.0)
        {
            return new UncertaintyScalar(
                centralValue,
                new NormalDistributionUncertaintyModel(Convert.ToDouble(centralValue), standardDeviation, coverageFactor));
        }
    }
}
#endif
