namespace VividOrange.Uncertainties
{
    public interface IAbsoluteUncertainty<T> : IUncertainty<T>
    {
        public T AbsoluteUncertaintyValue { get; }
    }
}
