using MagmaWorks.Uncertainties.Models;

namespace MagmaWorks.Uncertainties.Tests
{
    public class UncertaintyQuantityConstructorTests
    {
        [Fact]
        public void AbsoluteUncertaintyConstructor_UsesUnitConversion_Correctly()
        {
            var central = Length.FromMeters(100);
            var uncertainty = Length.FromCentimeters(50); // 0.5 m

            var uq = new UncertaintyQuantity<Length>(central, uncertainty);

            Assert.Equal(100, uq.CentralValue.Meters, 6);
            Assert.IsType<AbsoluteUncertaintyModel>(uq.Model);

            var absModel = (AbsoluteUncertaintyModel)uq.Model;
            Assert.InRange(absModel.AbsoluteUncertainty, 0.49, 0.51);
        }

        [Fact]
        public void RelativeUncertaintyConstructor_AssignsCorrectly()
        {
            var central = Length.FromMeters(100);
            var rel = Ratio.FromPercent(10);

            var uq = new UncertaintyQuantity<Length>(central, rel);

            Assert.Equal(100, uq.CentralValue.Meters, 6);
            Assert.IsType<RelativeUncertaintyModel>(uq.Model);

            var relModel = (RelativeUncertaintyModel)uq.Model;
            Assert.InRange(relModel.RelativeUncertainty, 0.099, 0.101);
        }

        [Fact]
        public void IntervalUncertaintyConstructor_AssignsBounds()
        {
            var central = Length.FromMeters(100);

            var uq = new UncertaintyQuantity<Length>(central, 80, 120);

            Assert.Equal(100, uq.CentralValue.Meters, 6);
            Assert.IsType<IntervalUncertaintyModel>(uq.Model);

            var intervalModel = (IntervalUncertaintyModel)uq.Model;
            Assert.Equal(80, intervalModel.LowerBound);
            Assert.Equal(120, intervalModel.UpperBound);
        }

        [Fact]
        public void NormalDistributionUncertaintyConstructor_AssignsValues()
        {
            var central = Length.FromMeters(100);

            var uq = new UncertaintyQuantity<Length>(central, 5);

            Assert.Equal(100, uq.CentralValue.Meters, 6);
            Assert.IsType<NormalDistributionUncertaintyModel>(uq.Model);

            var normalModel = (NormalDistributionUncertaintyModel)uq.Model;
            Assert.Equal(100, normalModel.Mean, 6);
            Assert.Equal(5, normalModel.StandardDeviation, 6);
            Assert.Equal(3, normalModel.CoverageFactor); // default coverage factor
        }
    }
}
