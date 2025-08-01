namespace MagmaWorks.Uncertainties.Tests
{
    public class NormalDistributionUncertaintyModelTests
    {
        [Fact]
        public void Scalar_WithNormalDistribution_HasExpectedBounds()
        {
            UncertaintyScalar<double> value = 0.0.WithNormalDistributionUncertainty(2.0); // μ = 0, σ = 2

            Assert.Equal(0.0, value.CentralValue);
            Assert.Equal(-6.0, value.LowerBound, 6); // ±3σ
            Assert.Equal(6.0, value.UpperBound, 6);
        }

        [Fact]
        public void Quantity_WithNormalDistribution_HasExpectedBounds()
        {
            UncertaintyQuantity<Speed> speed = Speed.FromKilometersPerHour(90).WithNormalDistributionUncertainty(5.0);

            Assert.Equal(90, speed.CentralValue.KilometersPerHour, 6);
            Assert.Equal(75, speed.LowerBound.KilometersPerHour, 6);
            Assert.Equal(105, speed.UpperBound.KilometersPerHour, 6);
        }

        [Fact]
        public void NormalDistribution_WithNegativeStdDev_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                UncertaintyScalar<double> _ = 1.0.WithNormalDistributionUncertainty(-1.0);
            });
        }

        [Fact]
        public void Scalar_Addition_WithNormalDistribution_CombinesCorrectly()
        {
            var a = 100.0.WithNormalDistributionUncertainty(5); // σ₁ = 5
            var b = 50.0.WithNormalDistributionUncertainty(3);  // σ₂ = 3

            var result = a.Add(b); // σ = sqrt(5² + 3²) ≈ 5.831

            Assert.Equal(150.0, result.CentralValue, 6);
            Assert.InRange(result.LowerBound, 132.5, 132.6); // 150 - 3*5.831
            Assert.InRange(result.UpperBound, 167.4, 167.5); // 150 + 3*5.831
        }

        [Fact]
        public void Quantity_Addition_WithNormalDistribution_CombinesCorrectly()
        {
            var a = Length.FromMeters(60).WithNormalDistributionUncertainty(2); // σ₁ = 2
            var b = Length.FromMeters(30).WithNormalDistributionUncertainty(1); // σ₂ = 1

            var result = a.Add(b); // σ = sqrt(2² + 1²) ≈ 2.236

            Assert.Equal(90.0, result.CentralValue.Meters, 6);
            Assert.InRange(result.LowerBound.Meters, 83.2, 83.3); // 90 - 3*2.236
            Assert.InRange(result.UpperBound.Meters, 96.7, 96.8); // 90 + 3*2.236
        }

        [Fact]
        public void Scalar_Subtraction_WithNormalDistribution_CombinesCorrectly()
        {
            var a = 100.0.WithNormalDistributionUncertainty(5); // σ₁ = 5
            var b = 50.0.WithNormalDistributionUncertainty(2);  // σ₂ = 2

            var result = a.Subtract(b); // σ = sqrt(5² + 2²) ≈ 5.385

            Assert.Equal(50.0, result.CentralValue, 6);
            Assert.InRange(result.LowerBound, 33.8, 33.9); // 50 - 3*5.385
            Assert.InRange(result.UpperBound, 66.1, 66.2); // 50 + 3*5.385
        }

        [Fact]
        public void Quantity_Subtraction_WithNormalDistribution_CombinesCorrectly()
        {
            var a = Speed.FromKilometersPerHour(90).WithNormalDistributionUncertainty(4); // σ₁ = 4
            var b = Speed.FromKilometersPerHour(30).WithNormalDistributionUncertainty(2); // σ₂ = 2

            var result = a.Subtract(b); // σ = sqrt(4² + 2²) = sqrt(20) ≈ 4.472

            Assert.Equal(60.0, result.CentralValue.KilometersPerHour, 6);
            Assert.InRange(result.LowerBound.KilometersPerHour, 46.5, 46.6); // 60 - 3*4.472
            Assert.InRange(result.UpperBound.KilometersPerHour, 73.4, 73.5); // 60 + 3*4.472
        }
    }
}
