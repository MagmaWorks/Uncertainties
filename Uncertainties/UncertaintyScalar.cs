#if NET8_0_OR_GREATER
using System;
using System.Numerics;

namespace MagmaWorks.Uncertainties
{
    public class UncertaintyScalar<T> : IUncertainty<T> where T : INumber<T>
    {
        public T CentralValue { get; }
        public IUncertaintyModel Model { get; }
        public T LowerBound => T.CreateChecked(Model.LowerBound);
        public T UpperBound => T.CreateChecked(Model.UpperBound);

        public UncertaintyScalar(T centralValue, IUncertaintyModel model)
        {
            CentralValue = centralValue;
            Model = model;
        }

        internal UncertaintyScalar<T> CloneWithTransformedModel(Func<double, double> transform)
        {
            var newModel = Model.PropagateUnary(transform);
            return new UncertaintyScalar<T>((T)(object)newModel.CentralValue, newModel);
        }
    }
}
#endif
