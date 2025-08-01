using System;
using System.Linq;

namespace MagmaWorks.Uncertainties.Models
{
    public class AbsoluteUncertaintyModel : IUncertaintyModel
    {
        public double CentralValue { get; }
        public double AbsoluteUncertainty { get; }

        public AbsoluteUncertaintyModel(double centralValue, double uncertainty)
        {
            CentralValue = centralValue;
            AbsoluteUncertainty = uncertainty;
        }

        public double LowerBound => CentralValue - AbsoluteUncertainty;
        public double UpperBound => CentralValue + AbsoluteUncertainty;

        public IUncertaintyModel PropagateBinary(IUncertaintyModel other, Func<double, double, double> op)
        {
            if (other is not AbsoluteUncertaintyModel abs)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            var thisLower = CentralValue - AbsoluteUncertainty;
            var thisUpper = CentralValue + AbsoluteUncertainty;

            var otherLower = abs.CentralValue - abs.AbsoluteUncertainty;
            var otherUpper = abs.CentralValue + abs.AbsoluteUncertainty;

            var candidates = new[]
            {
                op(thisLower, otherLower),
                op(thisLower, otherUpper),
                op(thisUpper, otherLower),
                op(thisUpper, otherUpper)
            };

            return new IntervalUncertaintyModel(candidates.Min(), candidates.Max());
        }

        public IUncertaintyModel PropagateUnary(Func<double, double> op)
        {
            var newCentral = op(CentralValue);
            var factor = Math.Abs(op(1.0)); // scale uncertainty
            var newUncertainty = AbsoluteUncertainty * factor;
            return new AbsoluteUncertaintyModel(newCentral, newUncertainty);
        }
    }
}
