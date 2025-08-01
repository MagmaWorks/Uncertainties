namespace MagmaWorks.Uncertainties
{
    public interface IUncertainty<T>
    {
        T CentralValue { get; }
        T LowerBound { get; }
        T UpperBound { get; }

        IUncertaintyModel Model { get; }
    }
}
