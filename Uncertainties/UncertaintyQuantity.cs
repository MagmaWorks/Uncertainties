using MagmaWorks.Uncertainties.Utility;
using UnitsNet;

namespace MagmaWorks.Uncertainties
{
    public class UncertaintyQuantity<TQuantity> : IUncertainty<TQuantity> where TQuantity : IQuantity
    {
        public TQuantity CentralValue { get; }
        public IUncertaintyModel Model { get; }
        public TQuantity LowerBound => (TQuantity)CentralValue.ToUnit(CentralValue.Unit).WithValue(Model.LowerBound);
        public TQuantity UpperBound => (TQuantity)CentralValue.ToUnit(CentralValue.Unit).WithValue(Model.UpperBound);

        public UncertaintyQuantity(TQuantity value, IUncertaintyModel model)
        {
            CentralValue = value;
            Model = model;
        }
    }
}
