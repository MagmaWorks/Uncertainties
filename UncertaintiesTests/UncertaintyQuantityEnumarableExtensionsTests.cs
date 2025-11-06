namespace VividOrange.Uncertainties.Tests
{
    public class UncertaintyQuantityEnumarableExtensionsTests
    {
        [Fact]
        public void Sum_WithOnlyNormalDistributionUncertainties()
        {
            // Assemble
            var u1 = new NormalDistributionUncertaintyQuantity<Mass>(
                Mass.FromKilograms(100),
                Mass.FromKilograms(10),
                coverageFactor: 2);

            var u2 = new NormalDistributionUncertaintyQuantity<Mass>(
                Mass.FromKilograms(200),
                Mass.FromKilograms(20),
                coverageFactor: 3);

            var list = new List<IUncertainty<Mass>> { u1, u2 };

            // Act
            IUncertainty<Mass> result = list.Sum();

            // Assert
            Assert.IsType<NormalDistributionUncertaintyQuantity<Mass>>(result);

            var normal = (NormalDistributionUncertaintyQuantity<Mass>)result;
            Assert.Equal(300, normal.CentralValue.Kilograms, 6);
            Assert.InRange(normal.StandardDeviation.Kilograms, 22.35, 22.37); // sqrt(10² + 20²) = 22.36
            Assert.Equal(3, normal.CoverageFactor);
        }

        [Fact]
        public void Sum_WithIntervalUncertainties_ComputesConservativeSum()
        {
            var u1 = new IntervalUncertaintyQuantity<Length>(
                Length.FromMeters(10),
                Length.FromMeters(8),
                Length.FromMeters(12));

            var u2 = new IntervalUncertaintyQuantity<Length>(
                Length.FromMeters(20),
                Length.FromMeters(18),
                Length.FromMeters(22));

            var list = new List<IUncertainty<Length>> { u1, u2 };

            IUncertainty<Length> result = list.Sum();

            Assert.IsType<IntervalUncertaintyQuantity<Length>>(result);

            var interval = (IntervalUncertaintyQuantity<Length>)result;
            Assert.Equal(30, interval.CentralValue.Meters, 6);
            Assert.Equal(26, interval.LowerBound.Meters, 6);
            Assert.Equal(34, interval.UpperBound.Meters, 6);
        }

        [Fact]
        public void Sum_WithMixedUncertaintyModels_FallsBackToIntervalSummation()
        {
            var normal = new NormalDistributionUncertaintyQuantity<Length>(
                Length.FromMeters(10),
                Length.FromMeters(1),
                coverageFactor: 2);

            var interval = new IntervalUncertaintyQuantity<Length>(
                Length.FromMeters(5),
                Length.FromMeters(4),
                Length.FromMeters(6));

            var list = new List<IUncertainty<Length>> { normal, interval };

            IUncertainty<Length> result = list.Sum();

            Assert.IsType<IntervalUncertaintyQuantity<Length>>(result);

            var intervalResult = (IntervalUncertaintyQuantity<Length>)result;
            Assert.Equal(15, intervalResult.CentralValue.Meters, 6);
            Assert.Equal(12, intervalResult.LowerBound.Meters, 6); // 8 + 4
            Assert.Equal(18, intervalResult.UpperBound.Meters, 6); // 12 + 6
        }

        [Fact]
        public void Sum_WithDifferentUnits_ConvertsToFirstUnit()
        {
            var u1 = new NormalDistributionUncertaintyQuantity<Length>(
                Length.FromMeters(10),
                Length.FromMeters(1),
                coverageFactor: 2);

            var u2 = new NormalDistributionUncertaintyQuantity<Length>(
                Length.FromCentimeters(1000), // 10 m
                Length.FromCentimeters(10),   // 0.1 m
                coverageFactor: 2);

            var list = new List<IUncertainty<Length>> { u1, u2 };

            IUncertainty<Length> result = list.Sum();

            Assert.IsType<NormalDistributionUncertaintyQuantity<Length>>(result);

            var normal = (NormalDistributionUncertaintyQuantity<Length>)result;
            Assert.InRange(normal.CentralValue.Meters, 19.9, 20.1);
            Assert.InRange(normal.StandardDeviation.Meters, 1.00, 1.05);
        }
    }
}
