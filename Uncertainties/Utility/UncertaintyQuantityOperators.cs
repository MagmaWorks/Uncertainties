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

        public static UncertaintyQuantity<TQuantity> Multiply<TQuantity>(
            this UncertaintyQuantity<TQuantity> a, double factor)
            where TQuantity : IQuantity
        {
            var newQuantity = Quantity.From((double)a.CentralValue.Value * factor, a.CentralValue.Unit);
            var newModel = a.Model.PropagateUnary(x => x * factor);

            return new UncertaintyQuantity<TQuantity>((TQuantity)newQuantity, newModel);
        }

        public static UncertaintyQuantity<TQuantity> Divide<TQuantity>(
            this UncertaintyQuantity<TQuantity> a, double divisor)
            where TQuantity : IQuantity
        {
            var newQuantity = Quantity.From((double)a.CentralValue.Value / divisor, a.CentralValue.Unit);
            var newModel = a.Model.PropagateUnary(x => x / divisor);

            return new UncertaintyQuantity<TQuantity>((TQuantity)newQuantity, newModel);
        }
    }
}
