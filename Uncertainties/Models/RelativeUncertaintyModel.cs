using System;
using System.Linq;

namespace MagmaWorks.Uncertainties.Models
{
    public class RelativeUncertaintyModel : IUncertaintyModel
    {
        public double CentralValue { get; }
        public double RelativeUncertainty { get; }

        public RelativeUncertaintyModel(double centralValue, double relativeUncertainty)
        {
            CentralValue = centralValue;
            RelativeUncertainty = relativeUncertainty;
        }

        public double LowerBound => CentralValue * (1 - RelativeUncertainty);
        public double UpperBound => CentralValue * (1 + RelativeUncertainty);

        public IUncertaintyModel PropagateBinary(IUncertaintyModel other, Func<double, double, double> op)
        {
            if (other is not RelativeUncertaintyModel rel)
                throw new InvalidOperationException("Incompatible uncertainty model.");

            double thisLower = CentralValue * (1 - RelativeUncertainty);
            double thisUpper = CentralValue * (1 + RelativeUncertainty);

            double otherLower = rel.CentralValue * (1 - rel.RelativeUncertainty);
            double otherUpper = rel.CentralValue * (1 + rel.RelativeUncertainty);

            // Try all combinations of bounds
            var candidates = new[]
            {
                op(thisLower, otherLower),
                op(thisLower, otherUpper),
                op(thisUpper, otherLower),
                op(thisUpper, otherUpper)
            };

            double resultLower = candidates.Min();
            double resultUpper = candidates.Max();

            return new IntervalUncertaintyModel(resultLower, resultUpper);
        }

        public IUncertaintyModel PropagateUnary(Func<double, double> op)
        {
            double result = op(CentralValue);
            return new RelativeUncertaintyModel(result, RelativeUncertainty);
        }
    }
}
