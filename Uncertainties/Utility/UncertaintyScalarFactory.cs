#if NET8_0_OR_GREATER
using System;
using System.Numerics;
using MagmaWorks.Uncertainties.Models;

namespace MagmaWorks.Uncertainties.Utility;

public static class UncertaintyScalarFactory
{
    public static UncertaintyScalar<T> WithAbsoluteUncertainty<T>(
        this T value, double absoluteUncertainty)
        where T : INumber<T>
    {
        var model = new AbsoluteUncertaintyModel(Convert.ToDouble(value), absoluteUncertainty);
        return new UncertaintyScalar<T>(value, model);
    }

    public static UncertaintyScalar<T> WithRelativeUncertainty<T>(
        this T value, double relativeUncertainty)
        where T : INumber<T>
    {
        var model = new RelativeUncertaintyModel(Convert.ToDouble(value), relativeUncertainty);
        return new UncertaintyScalar<T>(value, model);
    }

    public static UncertaintyScalar<T> WithIntervalUncertainty<T>(
        this T value, double lowerBound, double upperBound)
        where T : INumber<T>
    {
        var model = new IntervalUncertaintyModel(lowerBound, upperBound);
        return new UncertaintyScalar<T>(value, model);
    }

    public static UncertaintyScalar<T> WithNormalDistributionUncertainty<T>(
        this T value, double stdDev)
        where T : INumber<T>
    {
        var model = new NormalDistributionUncertaintyModel(Convert.ToDouble(value), stdDev);
        return new UncertaintyScalar<T>(value, model);
    }
}
#endif
