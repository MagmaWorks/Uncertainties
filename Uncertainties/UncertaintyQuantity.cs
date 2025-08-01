using System;
using MagmaWorks.Uncertainties.Models;
using MagmaWorks.Uncertainties.Utility;
using UnitsNet;

namespace MagmaWorks.Uncertainties
{
    public class UncertaintyQuantity<TQuantity> : IUncertainty<TQuantity> where TQuantity : IQuantity
    {
        public TQuantity CentralValue { get; }
        public IUncertaintyModel Model { get; }
        public TQuantity LowerBound => (TQuantity)CentralValue.ToUnit(CentralValue.Unit).WithValue(Model.LowerBound);
        public TQuantity UpperBound => (TQuantity)CentralValue.ToUnit(CentralValue.Unit).WithValue(Model.UpperBound);

        public UncertaintyQuantity(TQuantity centralValue, IUncertaintyModel model)
        {
            CentralValue = centralValue;
            Model = model;
        }

        // Absolute uncertainty constructor with unit conversion
        public UncertaintyQuantity(TQuantity centralValue, TQuantity absoluteUncertainty)
            : this(centralValue,
                   new AbsoluteUncertaintyModel(
                       (double)centralValue.Value,
                       absoluteUncertainty.As(centralValue.Unit)))
        {
        }

        // Relative uncertainty using Ratio, no unit conversion needed
        public UncertaintyQuantity(TQuantity centralValue, Ratio relativeUncertainty)
            : this(centralValue,
                   new RelativeUncertaintyModel(
                       (double)centralValue.Value,
                       relativeUncertainty.DecimalFractions))
        {
        }

        // Interval uncertainty with raw bounds
        public UncertaintyQuantity(TQuantity centralValue, double lowerBound, double upperBound)
            : this(centralValue, new IntervalUncertaintyModel(lowerBound, upperBound))
        {
        }

        // Normal distribution with std dev in same units as central value
        public UncertaintyQuantity(TQuantity centralValue, double standardDeviation)
            : this(centralValue, new NormalDistributionUncertaintyModel((double)centralValue.Value, standardDeviation))
        {
        }

        public static UncertaintyQuantity<TQuantity> FromAbsoluteUncertainty(
            TQuantity centralValue, TQuantity absoluteUncertainty)
        {
            return new UncertaintyQuantity<TQuantity>(
                centralValue,
                new AbsoluteUncertaintyModel(
                    (double)centralValue.Value,
                    absoluteUncertainty.As(centralValue.Unit)));
        }

        public static UncertaintyQuantity<TQuantity> FromRelativeUncertainty(
            TQuantity centralValue,
            Ratio relativeUncertainty)
        {
            return new UncertaintyQuantity<TQuantity>(
                centralValue,
                new RelativeUncertaintyModel(
                    (double)centralValue.Value,
                    relativeUncertainty.DecimalFractions));
        }

        public static UncertaintyQuantity<TQuantity> FromIntervalUncertainty(
            TQuantity centralValue, double lowerBound, double upperBound)
        {
            return new UncertaintyQuantity<TQuantity>(
                centralValue,
                new IntervalUncertaintyModel(lowerBound, upperBound));
        }

        public static UncertaintyQuantity<TQuantity> FromNormalDistributionUncertainty(
            TQuantity centralValue, double standardDeviation, double coverageFactor = 3.0)
        {
            return new UncertaintyQuantity<TQuantity>(
                centralValue,
                new NormalDistributionUncertaintyModel(
                    (double)centralValue.Value,
                    standardDeviation,
                    coverageFactor));
        }

        internal UncertaintyQuantity<TQuantity> CloneWithTransformedModel(Func<double, double> transform)
        {
            var newModel = Model.PropagateUnary(transform);
            var newQuantity = Quantity.From((double)newModel.CentralValue, CentralValue.Unit);
            return new UncertaintyQuantity<TQuantity>((TQuantity)newQuantity, newModel);
        }
    }
}
