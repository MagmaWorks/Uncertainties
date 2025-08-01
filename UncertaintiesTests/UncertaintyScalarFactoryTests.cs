using MagmaWorks.Uncertainties.Models;

namespace MagmaWorks.Uncertainties.Tests
{
    public class UncertaintyScalarFactoryTests
    {
        [Fact]
        public void FromAbsoluteUncertainty_CreatesCorrectModel()
        {
            double central = 200.0;
            double absUnc = 10.0;

            var us = UncertaintyScalar.FromAbsoluteUncertainty(central, absUnc);

            Assert.Equal(200.0, us.CentralValue);
            Assert.IsType<AbsoluteUncertaintyModel>(us.Model);

            var model = (AbsoluteUncertaintyModel)us.Model;
            Assert.Equal(10.0, model.AbsoluteUncertainty);
        }

        [Fact]
        public void FromRelativeUncertainty_CreatesCorrectModel()
        {
            double central = 200.0;
            double relUnc = 0.1;

            var us = UncertaintyScalar.FromRelativeUncertainty(central, relUnc);

            Assert.Equal(200.0, us.CentralValue);
            Assert.IsType<RelativeUncertaintyModel>(us.Model);

            var model = (RelativeUncertaintyModel)us.Model;
            Assert.InRange(model.RelativeUncertainty, 0.099, 0.101);
        }

        [Fact]
        public void FromIntervalUncertainty_CreatesCorrectModel()
        {
            double central = 200.0;

            var us = UncertaintyScalar.FromIntervalUncertainty(central, 180, 220);

            Assert.Equal(200.0, us.CentralValue);
            Assert.IsType<IntervalUncertaintyModel>(us.Model);

            var model = (IntervalUncertaintyModel)us.Model;
            Assert.Equal(180, model.LowerBound);
            Assert.Equal(220, model.UpperBound);
        }

        [Fact]
        public void FromNormalDistributionUncertainty_CreatesCorrectModel()
        {
            double central = 200.0;

            var us = UncertaintyScalar.FromNormalDistributionUncertainty(central, 15, coverageFactor: 1.96);

            Assert.Equal(200.0, us.CentralValue);
            Assert.IsType<NormalDistributionUncertaintyModel>(us.Model);

            var model = (NormalDistributionUncertaintyModel)us.Model;
            Assert.Equal(200.0, model.Mean, 6);
            Assert.Equal(15.0, model.StandardDeviation, 6);
            Assert.Equal(1.96, model.CoverageFactor);
        }
    }
}
