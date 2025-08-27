using System;
using UnitsNet;

namespace MagmaWorks.Uncertainties.Quantities.Utility
{
    public static class UncertaintyQuantityOperators
    {
        public static IUncertainty<TQuantity> Add<TQuantity>(
            this UncertaintyQuantity<TQuantity> a, UncertaintyQuantity<TQuantity> b)
            where TQuantity : IQuantity
        {
            if (a.CentralValue.QuantityInfo.BaseUnitInfo.QuantityName !=
                b.CentralValue.QuantityInfo.BaseUnitInfo.QuantityName)
                throw new InvalidOperationException("Incompatible units.");

            return a.PropagateBinary(b, (x, y) => x.Add(y));
        }

        public static IUncertainty<TQuantity> Subtract<TQuantity>(
            this UncertaintyQuantity<TQuantity> a, UncertaintyQuantity<TQuantity> b)
            where TQuantity : IQuantity
        {
            if (a.CentralValue.QuantityInfo.BaseUnitInfo.QuantityName !=
                b.CentralValue.QuantityInfo.BaseUnitInfo.QuantityName)
                throw new InvalidOperationException("Incompatible units.");

            return a.PropagateBinary(b, (x, y) => x.Subtract(y));
        }

        public static IUncertainty<TQuantity> Multiply<TQuantity>(
            this UncertaintyQuantity<TQuantity> a, double factor)
            where TQuantity : IQuantity
        {
            return a.PropagateUnary(x => x * factor);
        }

        public static IUncertainty<TQuantity> Divide<TQuantity>(
            this UncertaintyQuantity<TQuantity> a, double divisor)
            where TQuantity : IQuantity
        {
            return a.PropagateUnary(x => x / divisor);
        }
    }
}
