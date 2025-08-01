using System;

namespace MagmaWorks.Uncertainties
{
    public interface IUncertaintyModel
    {
        double CentralValue { get; }
        double LowerBound { get; }
        double UpperBound { get; }

        IUncertaintyModel PropagateBinary(IUncertaintyModel other, Func<double, double, double> operation);
        IUncertaintyModel PropagateUnary(Func<double, double> operation);
    }
}
