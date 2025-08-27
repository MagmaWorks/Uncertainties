using System;
using MagmaWorks.Taxonomy.Serialization;

namespace MagmaWorks.Uncertainties
{
    public interface IUncertainty<T> : ITaxonomySerializable
    {
        T CentralValue { get; }
        T LowerBound { get; }
        T UpperBound { get; }

        IUncertainty<T> PropagateBinary(IUncertainty<T> other, Func<T, T, T> operation);
        IUncertainty<T> PropagateUnary(Func<double, double> operation);
    }
}
