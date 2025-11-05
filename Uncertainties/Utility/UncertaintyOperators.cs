namespace VividOrange.Uncertainties.Utility
{
    public static class UncertaintyOperators
    {
        public static Uncertainty Add<T>(this IUncertainty<double> a, T b)
            where T : IUncertainty<double>
        {
            if (b is Uncertainty bModel)
            {
                return new Uncertainty(
                    a.PropagateBinary(bModel.UncertaintyModel, (x, y) => x + y));
            }

            return new Uncertainty(a.PropagateBinary(b, (x, y) => x + y));
        }

        public static Uncertainty Subtract<T>(this T a, T b)
            where T : IUncertainty<double>
        {
            if (b is Uncertainty bModel)
            {
                return new Uncertainty(
                    a.PropagateBinary(bModel.UncertaintyModel, (x, y) => x - y));
            }

            return new Uncertainty(a.PropagateBinary(b, (x, y) => x - y));
        }

        public static Uncertainty Multiply<T>(this T a, double factor)
        where T : IUncertainty<double>
        {
            return new Uncertainty(a.PropagateUnary(x => x * factor));
        }

        public static Uncertainty Divide<T>(this T a, double divisor)
            where T : IUncertainty<double>
        {
            return new Uncertainty(a.PropagateUnary(x => x / divisor));
        }
    }
}
