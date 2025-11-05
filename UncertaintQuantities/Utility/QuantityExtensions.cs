using UnitsNet;

namespace VividOrange.Uncertainties.Quantities.Utility;

public static class QuantityExtensions
{
    internal static TQuantity WithValue<TQuantity>(this TQuantity quantity, double newValue)
        where TQuantity : IQuantity
    {
        return (TQuantity)Quantity.From(newValue, quantity.Unit);
    }

    internal static TQuantity Subtract<TQuantity>(this TQuantity quantity, TQuantity other)
        where TQuantity : IQuantity
    {
        return (TQuantity)Quantity.From((double)quantity.Value - other.As(quantity.Unit), quantity.Unit);
    }

    internal static TQuantity Add<TQuantity>(this TQuantity quantity, TQuantity other)
        where TQuantity : IQuantity
    {
        return (TQuantity)Quantity.From((double)quantity.Value + other.As(quantity.Unit), quantity.Unit);
    }

    internal static double Divide<TQuantity>(this TQuantity quantity, TQuantity other)
        where TQuantity : IQuantity
    {
        return (double)quantity.Value / other.As(quantity.Unit);
    }

    internal static TQuantity Multiply<TQuantity>(this TQuantity quantity, double other)
        where TQuantity : IQuantity
    {
        return (TQuantity)Quantity.From((double)quantity.Value * other, quantity.Unit);
    }
}
