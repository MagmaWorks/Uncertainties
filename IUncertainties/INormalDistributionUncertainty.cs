namespace VividOrange.Uncertainties
{
    public interface INormalDistributionUncertainty<T> : IUncertainty<T>
    {
        public T StandardDeviation { get; }
        public double CoverageFactor { get; }
    }
}
