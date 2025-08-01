using MagmaWorks.Uncertainties.Models;
using UnitsNet;

namespace MagmaWorks.Uncertainties.Utility;

public static class UncertaintyQuantityFactory
{
    public static UncertaintyQuantity<TQuantity> WithAbsoluteUncertainty<TQuantity>(
        this TQuantity quantity, double absoluteUncertainty)
        where TQuantity : IQuantity
    {
        var model = new AbsoluteUncertaintyModel((double)quantity.Value, absoluteUncertainty);
        return new UncertaintyQuantity<TQuantity>(quantity, model);
    }

    public static UncertaintyQuantity<TQuantity> WithRelativeUncertainty<TQuantity>(
        this TQuantity quantity, double relativeUncertainty)
        where TQuantity : IQuantity
    {
        var model = new RelativeUncertaintyModel((double)quantity.Value, relativeUncertainty);
        return new UncertaintyQuantity<TQuantity>(quantity, model);
    }

    public static UncertaintyQuantity<TQuantity> WithIntervalUncertainty<TQuantity>(
        this TQuantity quantity, double lowerBound, double upperBound)
        where TQuantity : IQuantity
    {
        var model = new IntervalUncertaintyModel(lowerBound, upperBound);
        return new UncertaintyQuantity<TQuantity>(quantity, model);
    }

    public static UncertaintyQuantity<TQuantity> WithNormalDistributionUncertainty<TQuantity>(
        this TQuantity quantity, double stdDev)
        where TQuantity : IQuantity
    {
        var model = new NormalDistributionUncertaintyModel((double)quantity.Value, stdDev);
        return new UncertaintyQuantity<TQuantity>(quantity, model);
    }
}
