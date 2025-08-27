#if NET8_0_OR_GREATER
using System.Numerics;
using MagmaWorks.Uncertainties.Scalar;

namespace MagmaWorks.Uncertainties.Utility;

public static class GenericINumberExtensions
{
    public static AbsoluteUncertaintyScalar<T> WithAbsoluteUncertainty<T>(
        this T value, T absoluteUncertainty)
        where T : INumber<T>
    {
        return new AbsoluteUncertaintyScalar<T>(value, absoluteUncertainty);
    }

    public static RelativeUncertaintyScalar<T> WithRelativeUncertainty<T>(
        this T value, double relativeUncertainty)
        where T : INumber<T>
    {
        return new RelativeUncertaintyScalar<T>(value, relativeUncertainty);
    }

    public static IntervalUncertaintyScalar<T> WithIntervalUncertainty<T>(
        this T value, T lowerBound, T upperBound)
        where T : INumber<T>
    {
        return new IntervalUncertaintyScalar<T>(value, lowerBound, upperBound);
    }

    public static NormalDistributionUncertaintyScalar<T> WithNormalDistributionUncertainty<T>(
        this T value, T standardDeviation, double coverageFactor = 3.0)
        where T : INumber<T>
    {
        return new NormalDistributionUncertaintyScalar<T>(value, standardDeviation, coverageFactor);
    }
}
#endif
