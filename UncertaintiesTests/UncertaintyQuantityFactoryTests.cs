namespace MagmaWorks.Uncertainties.Tests
{
    public class UncertaintyQuantityFactoryTests
    {
        [Fact]
        public void FromAbsoluteUncertainty_CreatesCorrectModel()
        {
            var central = Length.FromMeters(50);
            var absUnc = Length.FromCentimeters(10); // 0.1 m

            var uq = UncertaintyQuantity<Length>.FromAbsoluteUncertainty(central, absUnc);

            Assert.Equal(50, uq.CentralValue.Meters, 6);
            Assert.IsType<AbsoluteUncertaintyQuantity<Length>>(uq.UncertaintyModel);

            var absModel = (AbsoluteUncertaintyQuantity<Length>)uq.UncertaintyModel;
            Assert.InRange(absModel.AbsoluteUncertaintyValue.Meters, 0.099, 0.101);
        }

        [Fact]
        public void FromRelativeUncertainty_CreatesCorrectModel()
        {
            var central = Length.FromMeters(50);
            var relUnc = Ratio.FromPercent(5);

            var uq = UncertaintyQuantity<Length>.FromRelativeUncertainty(central, relUnc);

            Assert.Equal(50, uq.CentralValue.Meters, 6);
            Assert.IsType<RelativeUncertaintyQuantity<Length>>(uq.UncertaintyModel);

            var relModel = (RelativeUncertaintyQuantity<Length>)uq.UncertaintyModel;
            Assert.InRange(relModel.RelativeUncertaintyValue, 0.049, 0.051);
        }

        [Fact]
        public void FromIntervalUncertainty_CreatesCorrectModel()
        {
            var central = Length.FromMeters(50);
            var lower = Length.FromMeters(40);
            var upper = Length.FromMeters(60);

            var uq = UncertaintyQuantity<Length>.FromIntervalUncertainty(central, lower, upper);

            Assert.Equal(50, uq.CentralValue.Meters, 6);
            Assert.IsType<IntervalUncertaintyQuantity<Length>>(uq.UncertaintyModel);

            var intervalModel = (IntervalUncertaintyQuantity<Length>)uq.UncertaintyModel;
            Assert.Equal(40, intervalModel.LowerBound.Meters);
            Assert.Equal(60, intervalModel.UpperBound.Meters);
        }

        [Fact]
        public void FromNormalDistributionUncertainty_CreatesCorrectModel()
        {
            var central = Length.FromMeters(50);
            var deviation = Length.FromMeters(3);

            var uq = UncertaintyQuantity<Length>.FromNormalDistributionUncertainty(
                central, deviation, coverageFactor: 2);

            Assert.Equal(50, uq.CentralValue.Meters, 6);
            Assert.IsType<NormalDistributionUncertaintyQuantity<Length>>(uq.UncertaintyModel);

            var normalModel = (NormalDistributionUncertaintyQuantity<Length>)uq.UncertaintyModel;
            Assert.Equal(50, normalModel.CentralValue.Meters, 6);
            Assert.Equal(3, normalModel.StandardDeviation.Meters, 6);
            Assert.Equal(2, normalModel.CoverageFactor);
        }
    }
}
