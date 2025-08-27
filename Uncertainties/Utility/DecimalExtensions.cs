using MagmaWorks.Uncertainties.Decimal;

namespace MagmaWorks.Uncertainties.Utility;

public static class DecimalExtensions
{
    public static AbsoluteUncertaintyDecimal WithAbsoluteUncertainty(
        this decimal value, decimal absoluteUncertainty)
    {
        return new AbsoluteUncertaintyDecimal(value, absoluteUncertainty);
    }

    public static RelativeUncertaintyDecimal WithRelativeUncertainty(
        this decimal value, double relativeUncertainty)
    {
        return new RelativeUncertaintyDecimal(value, relativeUncertainty);
    }

    public static IntervalUncertaintyDecimal WithIntervalUncertainty(
        this decimal value, decimal lowerBound, decimal upperBound)
    {
        return new IntervalUncertaintyDecimal(value, lowerBound, upperBound);
    }

    public static NormalDistributionUncertaintyDecimal WithNormalDistributionUncertainty(
        this decimal value, decimal standardDeviation, double coverageFactor = 3.0)
    {
        return new NormalDistributionUncertaintyDecimal(value, standardDeviation, coverageFactor);
    }
}
