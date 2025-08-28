namespace MagmaWorks.Uncertainties.Utility
{
    public static class UncertaintyOperators
    {
        public static Uncertainty Add<T>(this IUncertainty<double> a, T b)
            where T : IUncertainty<double>
        {
            return (Uncertainty)a.PropagateBinary(b, (x, y) => x + y);
        }

        public static Uncertainty Subtract<T>(this T a, T b)
            where T : IUncertainty<double>
        {
            return (Uncertainty)a.PropagateBinary(b, (x, y) => x - y);
        }

        public static Uncertainty Multiply<T>(this T a, double factor)
        where T : IUncertainty<double>
        {
            return (Uncertainty)a.PropagateUnary(x => x * factor);
        }

        public static Uncertainty Divide<T>(this T a, double divisor)
            where T : IUncertainty<double>
        {
            return (Uncertainty)a.PropagateUnary(x => x / divisor);
        }
    }
}
