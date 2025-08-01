#if NET8_0_OR_GREATER
using System.Numerics;

namespace MagmaWorks.Uncertainties.Utility
{
    public static class UncertaintyScalarOperators
    {
        public static UncertaintyScalar<T> Add<T>(
            this UncertaintyScalar<T> a, UncertaintyScalar<T> b)
            where T : INumber<T>
        {
            var result = a.CentralValue + b.CentralValue;
            var model = a.Model.PropagateBinary(b.Model, (x, y) => x + y);
            return new UncertaintyScalar<T>(result, model);
        }

        public static UncertaintyScalar<T> Subtract<T>(
            this UncertaintyScalar<T> a, UncertaintyScalar<T> b)
            where T : INumber<T>
        {
            var result = a.CentralValue - b.CentralValue;
            var model = a.Model.PropagateBinary(b.Model, (x, y) => x - y);
            return new UncertaintyScalar<T>(result, model);
        }
    }
}
#endif
