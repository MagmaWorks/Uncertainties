namespace MagmaWorks.Uncertainties
{
    public interface IRelativeUncertainty<T> : IUncertainty<T>
    {
        public double RelativeUncertaintyValue { get; }
    }
}
