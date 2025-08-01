using UnitsNet;

namespace MagmaWorks.Uncertainties.Utility;

public static class QuantityExtensions
{
    internal static TQuantity WithValue<TQuantity>(this TQuantity quantity, double newValue)
        where TQuantity : IQuantity
    {
        return (TQuantity)Quantity.From(newValue, quantity.Unit);
    }
}
