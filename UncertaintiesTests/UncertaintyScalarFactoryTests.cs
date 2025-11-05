namespace VividOrange.Uncertainties.Tests
{
    public class UncertaintyScalarFactoryTests
    {
        [Fact]
        public void FromAbsoluteUncertainty_CreatesCorrectModel()
        {
            double central = 200.0;
            double absUnc = 10.0;

            var us = UncertaintyScalar<double>.FromAbsoluteUncertainty(central, absUnc);

            Assert.Equal(200.0, us.CentralValue);
            Assert.IsType<AbsoluteUncertaintyScalar<double>>(us.UncertaintyModel);

            var model = (AbsoluteUncertaintyScalar<double>)us.UncertaintyModel;
            Assert.Equal(10.0, model.AbsoluteUncertaintyValue);
        }

        [Fact]
        public void FromRelativeUncertainty_CreatesCorrectModel()
        {
            double central = 200.0;
            double relUnc = 0.1;

            var us = UncertaintyScalar<double>.FromRelativeUncertainty(central, relUnc);

            Assert.Equal(200.0, us.CentralValue);
            Assert.IsType<RelativeUncertaintyScalar<double>>(us.UncertaintyModel);

            var model = (RelativeUncertaintyScalar<double>)us.UncertaintyModel;
            Assert.InRange(model.RelativeUncertaintyValue, 0.099, 0.101);
        }

        [Fact]
        public void FromIntervalUncertainty_CreatesCorrectModel()
        {
            double central = 200.0;

            var us = UncertaintyScalar<double>.FromIntervalUncertainty(central, 180, 220);

            Assert.Equal(200.0, us.CentralValue);
            Assert.IsType<IntervalUncertaintyScalar<double>>(us.UncertaintyModel);

            var model = (IntervalUncertaintyScalar<double>)us.UncertaintyModel;
            Assert.Equal(180, model.LowerBound);
            Assert.Equal(220, model.UpperBound);
        }

        [Fact]
        public void FromNormalDistributionUncertainty_CreatesCorrectModel()
        {
            double central = 200.0;

            var us = UncertaintyScalar<double>.
                FromNormalDistributionUncertainty(central, 15, coverageFactor: 1.96);

            Assert.Equal(200.0, us.CentralValue);
            Assert.IsType<NormalDistributionUncertaintyScalar<double>>(us.UncertaintyModel);

            var model = (NormalDistributionUncertaintyScalar<double>)us.UncertaintyModel;
            Assert.Equal(200.0, model.CentralValue, 6);
            Assert.Equal(15.0, model.StandardDeviation, 6);
            Assert.Equal(1.96, model.CoverageFactor);
        }
    }
}
