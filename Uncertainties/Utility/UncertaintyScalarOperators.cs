#if NET8_0_OR_GREATER
using System.Numerics;

namespace MagmaWorks.Uncertainties.Utility
{
    public static class UncertaintyScalarOperators
    {
        public static IUncertainty<T> Add<T>(
            this UncertaintyScalar<T> a, UncertaintyScalar<T> b)
            where T : INumber<T>
        {
            return a.PropagateBinary(b, (x, y) => x + y);
        }

        public static IUncertainty<T> Subtract<T>(
            this UncertaintyScalar<T> a, UncertaintyScalar<T> b)
            where T : INumber<T>
        {
            return a.PropagateBinary(b, (x, y) => x - y);
        }

        public static IUncertainty<T> Multiply<T>(
        this UncertaintyScalar<T> a, double factor)
        where T : INumber<T>
        {
            return a.PropagateUnary(x => x * factor);
        }

        public static IUncertainty<T> Divide<T>(
            this UncertaintyScalar<T> a, double divisor)
            where T : INumber<T>
        {
            return a.PropagateUnary(x => x / divisor);
        }
    }
}
#endif
