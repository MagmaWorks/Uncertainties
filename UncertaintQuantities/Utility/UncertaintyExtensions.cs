using System;
using System.Collections.Generic;
using System.Linq;
using UnitsNet;

namespace VividOrange.Uncertainties.Quantities.Utility
{
    public static class UncertaintyEnumerableExtensions
    {
        /// <summary>
        /// Sums a list of uncertainties.
        /// Uses statistical combination for normal distributions, 
        /// and conservative interval summation otherwise by
        /// treading all items as interval uncertainties.
        /// </summary>
        /// <typeparam name="TQuantity">Quantity type (e.g. Length, Mass, etc.)</typeparam>
        /// <param name="items">Enumerable of uncertainties to combine</param>
        /// <returns>A combined uncertainty in the same unit as the first list item</returns>
        public static IUncertainty<TQuantity> Sum<TQuantity>(
            this IEnumerable<IUncertainty<TQuantity>> items)
            where TQuantity : IQuantity
        {
            // Ensure matching quantity types (Length vs Mass, etc.)
            TQuantity firstInfo = items.FirstOrDefault().CentralValue;
            if (items.Any(x => x.CentralValue.QuantityInfo.UnitType != firstInfo.QuantityInfo.UnitType))
            {
                throw new InvalidOperationException("All uncertainties must be of the same physical quantity type.");
            }

            Enum canonicalUnit = firstInfo.Unit;
            var centralDoubles = items.Select(x => x.CentralValue.As(canonicalUnit)).ToList();
            var lowerDoubles = items.Select(x => x.LowerBound.As(canonicalUnit)).ToList();
            var upperDoubles = items.Select(x => x.UpperBound.As(canonicalUnit)).ToList();

            // If all are normal-distribution uncertainties -> do proper statistical combine
            if (items.All(u => u is INormalDistributionUncertainty<TQuantity>))
            {
                double coverage = 0;
                var stdDevs = new List<double>();
                foreach (INormalDistributionUncertainty<TQuantity> n in items)
                {
                    stdDevs.Add(SafeStdDevFromNormal(n, canonicalUnit));
                    if (n.CoverageFactor > coverage)
                    {
                        coverage = n.CoverageFactor;
                    }
                }

                return SumNormalDistributionDoubles<TQuantity>(centralDoubles, stdDevs, coverage, canonicalUnit);
            }

            return SumIntervalDoubles<TQuantity>(centralDoubles, lowerDoubles, upperDoubles, canonicalUnit);
        }

        private static IUncertainty<TQuantity> SumNormalDistributionDoubles<TQuantity>(
            List<double> centralValues, List<double> stdDevs, double coverage, Enum unit)
            where TQuantity : IQuantity
        {
            double meanSum = centralValues.Sum();
            double combinedSd = Math.Sqrt(stdDevs.Sum(s => s * s));

            var meanQty = (TQuantity)Quantity.From(meanSum, unit);
            var sdQty = (TQuantity)Quantity.From(combinedSd, unit);

            return new NormalDistributionUncertaintyQuantity<TQuantity>(meanQty, sdQty, coverage);
        }

        private static IUncertainty<TQuantity> SumIntervalDoubles<TQuantity>(
            IReadOnlyList<double> centralValues,
            IReadOnlyList<double> lowerBounds,
            IReadOnlyList<double> upperBounds,
            Enum unit)
            where TQuantity : IQuantity
        {
            var centralQty = (TQuantity)Quantity.From(centralValues.Sum(), unit);
            var lowerQty = (TQuantity)Quantity.From(lowerBounds.Sum(), unit);
            var upperQty = (TQuantity)Quantity.From(upperBounds.Sum(), unit);

            return new IntervalUncertaintyQuantity<TQuantity>(centralQty, lowerQty, upperQty);
        }

        private static double SafeStdDevFromNormal<TQuantity>(INormalDistributionUncertainty<TQuantity> n, Enum unit)
            where TQuantity : IQuantity
        {
            // If StandardDeviation supplied appears non-zero and finite, use it.
            double sd = n.StandardDeviation.As(unit);
            if (!double.IsNaN(sd) && !double.IsInfinity(sd) && Math.Abs(sd) > 0.0)
                return sd;

            // Otherwise, if we have bounds and coverage factor, compute sd from expanded uncertainty:
            // expanded (u) ≈ (upper - lower) / 2, then sigma = u / k
            try
            {
                double lower = n.LowerBound.As(unit);
                double upper = n.UpperBound.As(unit);
                double k = n.CoverageFactor;
                if (k <= 0 || double.IsNaN(k) || double.IsInfinity(k))
                    k = 3.0; // fallback, treat as 3σ if coverage not usable

                double expanded = (upper - lower) / 2.0;
                double computedSd = expanded / k;

                if (!double.IsNaN(computedSd) && !double.IsInfinity(computedSd) && computedSd >= 0.0)
                    return computedSd;
            }
            catch
            {
                // ignore and fallback below
            }

            // As an ultimate fallback, return 0 (effectively treat as deterministic)
            return 0.0;
        }
    }
}
