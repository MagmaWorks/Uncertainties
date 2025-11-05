namespace VividOrange.Uncertainties.Utility;

public static class DoubleExtensions
{
    public static AbsoluteUncertainty WithAbsoluteUncertainty(
        this double value, double absoluteUncertainty)
    {
        return new AbsoluteUncertainty(value, absoluteUncertainty);
    }

    public static RelativeUncertainty WithRelativeUncertainty(
        this double value, double relativeUncertainty)
    {
        return new RelativeUncertainty(value, relativeUncertainty);
    }

    public static IntervalUncertainty WithIntervalUncertainty(
        this double value, double lowerBound, double upperBound)
    {
        return new IntervalUncertainty(value, lowerBound, upperBound);
    }

    public static NormalDistributionUncertainty WithNormalDistributionUncertainty(
        this double value, double standardDeviation, double coverageFactor = 3.0)
    {
        return new NormalDistributionUncertainty(value, standardDeviation, coverageFactor);
    }
}
