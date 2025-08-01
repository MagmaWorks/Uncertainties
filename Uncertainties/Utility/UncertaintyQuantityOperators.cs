using System;
using UnitsNet;

namespace MagmaWorks.Uncertainties.Utility
{
    public static class UncertaintyQuantityOperators
    {
        public static UncertaintyQuantity<TQuantity> Add<TQuantity>(
            this UncertaintyQuantity<TQuantity> a, UncertaintyQuantity<TQuantity> b)
            where TQuantity : IQuantity
        {
            if (a.CentralValue.QuantityInfo.BaseUnitInfo.QuantityName !=
                b.CentralValue.QuantityInfo.BaseUnitInfo.QuantityName)
                throw new InvalidOperationException("Incompatible units.");

            var resultValue = Quantity.From(
                (double)a.CentralValue.Value + b.CentralValue.As(a.CentralValue.Unit),
                a.CentralValue.Unit);

            var resultModel = a.Model.PropagateBinary(b.Model, (x, y) => x + y);
            return new UncertaintyQuantity<TQuantity>((TQuantity)resultValue, resultModel);
        }

        public static UncertaintyQuantity<TQuantity> Subtract<TQuantity>(
            this UncertaintyQuantity<TQuantity> a, UncertaintyQuantity<TQuantity> b)
            where TQuantity : IQuantity
        {
            if (a.CentralValue.QuantityInfo.BaseUnitInfo.QuantityName !=
                b.CentralValue.QuantityInfo.BaseUnitInfo.QuantityName)
                throw new InvalidOperationException("Incompatible units.");

            var resultValue = Quantity.From(
                (double)a.CentralValue.Value - b.CentralValue.As(a.CentralValue.Unit),
                a.CentralValue.Unit);

            var resultModel = a.Model.PropagateBinary(b.Model, (x, y) => x - y);
            return new UncertaintyQuantity<TQuantity>((TQuantity)resultValue, resultModel);
        }
    }
}
