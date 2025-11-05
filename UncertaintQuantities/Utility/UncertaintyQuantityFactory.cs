using UnitsNet;

namespace VividOrange.Uncertainties.Quantities.Utility;

public static class UncertaintyQuantityFactory
{
    public static AbsoluteUncertaintyQuantity<TQuantity> WithAbsoluteUncertainty<TQuantity>(
        this TQuantity quantity, TQuantity absoluteUncertainty)
        where TQuantity : IQuantity
    {
        return new AbsoluteUncertaintyQuantity<TQuantity>(quantity, absoluteUncertainty);
    }

    public static AbsoluteUncertaintyQuantity<TQuantity> WithAbsoluteUncertainty<TQuantity>(
        this TQuantity quantity, double absoluteUncertainty)
        where TQuantity : IQuantity
    {
        return new AbsoluteUncertaintyQuantity<TQuantity>(
            quantity, quantity.WithValue(absoluteUncertainty));
    }

    public static RelativeUncertaintyQuantity<TQuantity> WithRelativeUncertainty<TQuantity>(
        this TQuantity quantity, Ratio relativeUncertainty)
        where TQuantity : IQuantity
    {
        return new RelativeUncertaintyQuantity<TQuantity>(quantity, relativeUncertainty);
    }

    public static RelativeUncertaintyQuantity<TQuantity> WithRelativeUncertainty<TQuantity>(
        this TQuantity quantity, double relativeUncertainty)
        where TQuantity : IQuantity
    {
        return new RelativeUncertaintyQuantity<TQuantity>(quantity, relativeUncertainty);
    }

    public static IntervalUncertaintyQuantity<TQuantity> WithIntervalUncertainty<TQuantity>(
        this TQuantity quantity, TQuantity lowerBound, TQuantity upperBound)
        where TQuantity : IQuantity
    {
        return new IntervalUncertaintyQuantity<TQuantity>(quantity, lowerBound, upperBound);
    }
    public static IntervalUncertaintyQuantity<TQuantity> WithIntervalUncertainty<TQuantity>(
        this TQuantity quantity, double lowerBound, double upperBound)
        where TQuantity : IQuantity
    {
        return new IntervalUncertaintyQuantity<TQuantity>(
            quantity, quantity.WithValue(lowerBound), quantity.WithValue(upperBound));
    }

    public static NormalDistributionUncertaintyQuantity<TQuantity>
        WithNormalDistributionUncertainty<TQuantity>(
        this TQuantity quantity, TQuantity standardDeviation, double coverageFactor = 3.0)
        where TQuantity : IQuantity
    {
        return new NormalDistributionUncertaintyQuantity<TQuantity>(
            quantity, standardDeviation, coverageFactor);
    }

    public static NormalDistributionUncertaintyQuantity<TQuantity>
        WithNormalDistributionUncertainty<TQuantity>(
        this TQuantity quantity, double standardDeviation, double coverageFactor = 3.0)
        where TQuantity : IQuantity
    {
        return new NormalDistributionUncertaintyQuantity<TQuantity>(
            quantity, quantity.WithValue(standardDeviation), coverageFactor);
    }
}
