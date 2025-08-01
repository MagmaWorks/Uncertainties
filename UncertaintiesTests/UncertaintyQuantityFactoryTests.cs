using MagmaWorks.Uncertainties.Models;

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
            Assert.IsType<AbsoluteUncertaintyModel>(uq.Model);

            var model = (AbsoluteUncertaintyModel)uq.Model;
            Assert.InRange(model.AbsoluteUncertainty, 0.099, 0.101);
        }

        [Fact]
        public void FromRelativeUncertainty_CreatesCorrectModel()
        {
            var central = Length.FromMeters(50);
            var relUnc = Ratio.FromPercent(5);

            var uq = UncertaintyQuantity<Length>.FromRelativeUncertainty(central, relUnc);

            Assert.Equal(50, uq.CentralValue.Meters, 6);
            Assert.IsType<RelativeUncertaintyModel>(uq.Model);

            var model = (RelativeUncertaintyModel)uq.Model;
            Assert.InRange(model.RelativeUncertainty, 0.049, 0.051);
        }

        [Fact]
        public void FromIntervalUncertainty_CreatesCorrectModel()
        {
            var central = Length.FromMeters(50);

            var uq = UncertaintyQuantity<Length>.FromIntervalUncertainty(central, 40, 60);

            Assert.Equal(50, uq.CentralValue.Meters, 6);
            Assert.IsType<IntervalUncertaintyModel>(uq.Model);

            var model = (IntervalUncertaintyModel)uq.Model;
            Assert.Equal(40, model.LowerBound);
            Assert.Equal(60, model.UpperBound);
        }

        [Fact]
        public void FromNormalDistributionUncertainty_CreatesCorrectModel()
        {
            var central = Length.FromMeters(50);

            var uq = UncertaintyQuantity<Length>.FromNormalDistributionUncertainty(central, 3, coverageFactor: 2);

            Assert.Equal(50, uq.CentralValue.Meters, 6);
            Assert.IsType<NormalDistributionUncertaintyModel>(uq.Model);

            var model = (NormalDistributionUncertaintyModel)uq.Model;
            Assert.Equal(50, model.Mean, 6);
            Assert.Equal(3, model.StandardDeviation, 6);
            Assert.Equal(2, model.CoverageFactor);
        }
    }
}
