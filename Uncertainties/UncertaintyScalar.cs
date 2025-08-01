#if NET8_0_OR_GREATER
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
    }
}
#endif
