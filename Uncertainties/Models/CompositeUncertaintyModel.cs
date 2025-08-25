using System;
using System.Collections.Generic;
using System.Linq;
using MagmaWorks.Uncertainties.Models;

namespace MagmaWorks.Uncertainties
{
    /// <summary>
    /// How to combine multiple uncertainties into a single composite.
    /// </summary>
    public enum CompositeAggregationMode
    {
        Auto,                   // All children normal -> QuadratureAsymmetric; else Envelope
        Envelope,               // Lower = min(L_i), Upper = max(U_i)
        QuadratureSymmetric,    // Half-width = sqrt(sum(max(dl,du)^2)) on both sides
        QuadratureAsymmetric    // Lower half-width = sqrt(sum(dl_i^2)), upper = sqrt(sum(du_i^2))
    }

    /// <summary>
    /// Composite model that stores many concrete uncertainties (all for the same measurand)
    /// and exposes a single IUncertaintyModel view over them.
    /// </summary>
    public sealed class CompositeUncertaintyModel<T> : IUncertaintyModel
    {
        /// <summary>
        /// Mutable list: you can keep adding or removing uncertainties at runtime.
        /// All must refer to the same measurand (i.e., consistent central values).
        /// </summary>
        public IList<IUncertainty<T>> Uncertainties { get; }

        /// <summary>
        /// Aggregation rule for computing lower/upper from children.
        /// </summary>
        public CompositeAggregationMode Mode { get; set; } = CompositeAggregationMode.Auto;

        /// <summary>
        /// Allowed absolute difference between children central values.
        /// </summary>
        public double CentralConsistencyTolerance { get; set; } = 1e-9;

        public CompositeUncertaintyModel(IList<IUncertainty<T>> uncertainties)
        {
            Uncertainties = uncertainties ?? new List<IUncertainty<T>>();
        }

        private IEnumerable<IUncertaintyModel> Models => Uncertainties.Select(u => u.Model);

        public double CentralValue => ComputeCentral();

        public double LowerBound => ComputeBounds().lower;

        public double UpperBound => ComputeBounds().upper;

        public IUncertaintyModel PropagateUnary(Func<double, double> op)
        {
            if (op is null) throw new ArgumentNullException(nameof(op));
            if (!Uncertainties.Any())
                throw new InvalidOperationException("Composite contains no uncertainties.");

            // Conservative envelope through the unary op: apply op to all child bounds
            var vals = new List<double>();
            foreach (var m in Models)
            {
                vals.Add(op(m.LowerBound));
                vals.Add(op(m.UpperBound));
            }

            var c = op(CentralValue);
            var lower = vals.Min();
            var upper = vals.Max();
            return new IntervalUncertaintyModel(c, lower, upper);
        }

        public IUncertaintyModel PropagateBinary(IUncertaintyModel other, Func<double, double, double> op)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            if (op is null) throw new ArgumentNullException(nameof(op));
            if (!Uncertainties.Any())
                throw new InvalidOperationException("Composite contains no uncertainties.");

            var c = op(this.CentralValue, other.CentralValue);

            // Cartesian product of child bounds; envelope the results.
            var rhs = (other as CompositeUncertaintyModel<T>)?.Models ?? new[] { other };
            var vals = new List<double>();

            foreach (var a in Models)
            {
                foreach (var b in rhs)
                {
                    vals.Add(op(a.LowerBound, b.LowerBound));
                    vals.Add(op(a.LowerBound, b.UpperBound));
                    vals.Add(op(a.UpperBound, b.LowerBound));
                    vals.Add(op(a.UpperBound, b.UpperBound));
                }
            }

            var lower = vals.Min();
            var upper = vals.Max();
            return new IntervalUncertaintyModel(c, lower, upper);
        }

        private double ComputeCentral()
        {
            if (!Uncertainties.Any())
                throw new InvalidOperationException("Composite contains no uncertainties.");

            var first = Models.First().CentralValue;

            // Enforce consistency; if you prefer averaging, replace with Average().
            if (Models.Any(m => Math.Abs(m.CentralValue - first) > CentralConsistencyTolerance))
                throw new InvalidOperationException("Child central values are inconsistent in composite.");

            return first;
        }

        private (double lower, double upper) ComputeBounds()
        {
            if (!Uncertainties.Any())
                throw new InvalidOperationException("Composite contains no uncertainties.");

            var c = ComputeCentral();

            var mode = Mode == CompositeAggregationMode.Auto
                ? (AreAllNormal() ? CompositeAggregationMode.QuadratureAsymmetric : CompositeAggregationMode.Envelope)
                : Mode;

            if (mode == CompositeAggregationMode.Envelope)
            {
                return (Models.Min(m => m.LowerBound), Models.Max(m => m.UpperBound));
            }

            // Quadrature modes: sum half-widths in quadrature
            double lw2 = 0.0, uw2 = 0.0;

            foreach (var m in Models)
            {
                var dl = Math.Max(0.0, c - m.LowerBound);     // lower half-width
                var du = Math.Max(0.0, m.UpperBound - c);     // upper half-width

                if (mode == CompositeAggregationMode.QuadratureSymmetric)
                {
                    var hw = Math.Max(dl, du);
                    lw2 += hw * hw;
                    uw2 += hw * hw;
                }
                else // Asymmetric
                {
                    lw2 += dl * dl;
                    uw2 += du * du;
                }
            }

            return (c - Math.Sqrt(lw2), c + Math.Sqrt(uw2));
        }

        private bool AreAllNormal() => Models.All(m => m is NormalDistributionUncertaintyModel);

        // If your IUncertaintyModel includes Distribution, expose null (no single distribution).
        // public IDistribution? Distribution => null;
    }
}
