namespace VividOrange.Uncertainties.Tests
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
            Assert.IsType<AbsoluteUncertaintyQuantity<Length>>(uq.UncertaintyModel);

            var absModel = (AbsoluteUncertaintyQuantity<Length>)uq.UncertaintyModel;
            Assert.InRange(absModel.AbsoluteUncertaintyValue.Meters, 0.49, 0.51);
        }

        [Fact]
        public void RelativeUncertaintyConstructor_AssignsCorrectly()
        {
            var central = Length.FromMeters(100);
            var rel = Ratio.FromPercent(10);

            var uq = new UncertaintyQuantity<Length>(central, rel);

            Assert.Equal(100, uq.CentralValue.Meters, 6);
            Assert.IsType<RelativeUncertaintyQuantity<Length>>(uq.UncertaintyModel);

            var relModel = (RelativeUncertaintyQuantity<Length>)uq.UncertaintyModel;
            Assert.InRange(relModel.RelativeUncertaintyValue, 0.099, 0.101);
        }

        [Fact]
        public void IntervalUncertaintyConstructor_AssignsBounds()
        {
            var central = Length.FromMeters(100);
            var lower = Length.FromMeters(80);
            var upper = Length.FromMeters(120);

            var uq = new UncertaintyQuantity<Length>(central, lower, upper);

            Assert.Equal(100, uq.CentralValue.Meters, 6);
            Assert.IsType<IntervalUncertaintyQuantity<Length>>(uq.UncertaintyModel);

            var intervalModel = (IntervalUncertaintyQuantity<Length>)uq.UncertaintyModel;
            Assert.Equal(80, intervalModel.LowerBound.Meters);
            Assert.Equal(120, intervalModel.UpperBound.Meters);
        }

        [Fact]
        public void NormalDistributionUncertaintyConstructor_AssignsValues()
        {
            var central = Length.FromMeters(100);
            var deviation = Length.FromMeters(5);

            var uq = new UncertaintyQuantity<Length>(central, deviation, 3.0);

            Assert.Equal(100, uq.CentralValue.Meters, 6);
            Assert.IsType<NormalDistributionUncertaintyQuantity<Length>>(uq.UncertaintyModel);

            var normalModel = (NormalDistributionUncertaintyQuantity<Length>)uq.UncertaintyModel;
            Assert.Equal(100, normalModel.CentralValue.Meters, 6);
            Assert.Equal(5, normalModel.StandardDeviation.Meters, 6);
            Assert.Equal(3, normalModel.CoverageFactor); // default coverage factor
        }
    }
}
