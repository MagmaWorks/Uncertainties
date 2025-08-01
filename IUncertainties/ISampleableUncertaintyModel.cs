using System;

namespace MagmaWorks.Uncertainties
{
    public interface ISampleableUncertaintyModel : IUncertaintyModel
    {
        double Sample(Random rng);
    }
}
