namespace VividOrange.Uncertainties.Tests
{
    public class NormalDistributionUncertaintyModelTests
    {
        [Fact]
        public void Scalar_WithNormalDistribution_HasExpectedBounds()
        {
            Uncertainty value = 0.0.WithNormalDistributionUncertainty(2.0); // μ = 0, σ = 2

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
                Uncertainty _ = 1.0.WithNormalDistributionUncertainty(-1.0);
            });
        }

        [Fact]
        public void Scalar_Addition_WithNormalDistribution_CombinesCorrectly()
        {
            Uncertainty a = 100.0.WithNormalDistributionUncertainty(5); // σ₁ = 5
            Uncertainty b = 50.0.WithNormalDistributionUncertainty(3);  // σ₂ = 3

            Uncertainty result = a.Add(b); // σ = sqrt(5² + 3²) ≈ 5.831

            Assert.Equal(150.0, result.CentralValue, 6);
            Assert.InRange(result.LowerBound, 132.5, 132.6); // 150 - 3*5.831
            Assert.InRange(result.UpperBound, 167.4, 167.5); // 150 + 3*5.831
        }

        [Fact]
        public void Quantity_Addition_WithNormalDistribution_CombinesCorrectly()
        {
            UncertaintyQuantity<Length> a = Length.FromMeters(60).WithNormalDistributionUncertainty(2); // σ₁ = 2
            UncertaintyQuantity<Length> b = Length.FromMeters(30).WithNormalDistributionUncertainty(1); // σ₂ = 1

            UncertaintyQuantity<Length> result = a.Add(b); // σ = sqrt(2² + 1²) ≈ 2.236

            Assert.Equal(90.0, result.CentralValue.Meters, 6);
            Assert.InRange(result.LowerBound.Meters, 83.2, 83.3); // 90 - 3*2.236
            Assert.InRange(result.UpperBound.Meters, 96.7, 96.8); // 90 + 3*2.236
        }

        [Fact]
        public void Scalar_Subtraction_WithNormalDistribution_CombinesCorrectly()
        {
            Uncertainty a = 100.0.WithNormalDistributionUncertainty(5); // σ₁ = 5
            Uncertainty b = 50.0.WithNormalDistributionUncertainty(2);  // σ₂ = 2

            Uncertainty result = a.Subtract(b); // σ = sqrt(5² + 2²) ≈ 5.385

            Assert.Equal(50.0, result.CentralValue, 6);
            Assert.InRange(result.LowerBound, 33.8, 33.9); // 50 - 3*5.385
            Assert.InRange(result.UpperBound, 66.1, 66.2); // 50 + 3*5.385
        }

        [Fact]
        public void Quantity_Subtraction_WithNormalDistribution_CombinesCorrectly()
        {
            UncertaintyQuantity<Speed> a = Speed.FromKilometersPerHour(90).WithNormalDistributionUncertainty(4); // σ₁ = 4
            UncertaintyQuantity<Speed> b = Speed.FromKilometersPerHour(30).WithNormalDistributionUncertainty(2); // σ₂ = 2

            UncertaintyQuantity<Speed> result = a.Subtract(b); // σ = sqrt(4² + 2²) = sqrt(20) ≈ 4.472

            Assert.Equal(60.0, result.CentralValue.KilometersPerHour, 6);
            Assert.InRange(result.LowerBound.KilometersPerHour, 46.5, 46.6); // 60 - 3*4.472
            Assert.InRange(result.UpperBound.KilometersPerHour, 73.4, 73.5); // 60 + 3*4.472
        }

        [Fact]
        public void Scalar_Multiplication_WithNormalDistribution_CombinesCorrectly()
        {
            Uncertainty a = 100.0.WithNormalDistributionUncertainty(5);
            double factor = 2.0;

            Uncertainty result = a.Multiply(factor);

            Assert.Equal(200.0, result.CentralValue, 6);
            Assert.InRange(result.LowerBound, 170.0, 170.1); // 200 - 3*10
            Assert.InRange(result.UpperBound, 230.0, 230.1); // 200 + 3*10
        }

        [Fact]
        public void Scalar_Division_WithNormalDistribution_CombinesCorrectly()
        {
            Uncertainty a = 100.0.WithNormalDistributionUncertainty(10);
            double divisor = 2.0;

            Uncertainty result = a.Divide(divisor);

            Assert.Equal(50.0, result.CentralValue, 6);
            Assert.InRange(result.LowerBound, 35.0, 35.1);
            Assert.InRange(result.UpperBound, 65.0, 65.1);
        }

        [Fact]
        public void Quantity_Multiplication_WithNormalDistribution_CombinesCorrectly()
        {
            UncertaintyQuantity<Length> a = Length.FromMeters(60).WithNormalDistributionUncertainty(2);
            double factor = 3.0;

            UncertaintyQuantity<Length> result = a.Multiply(factor);

            Assert.Equal(180.0, result.CentralValue.Meters, 6);
            Assert.InRange(result.LowerBound.Meters, 162.0, 162.1);
            Assert.InRange(result.UpperBound.Meters, 198.0, 198.1);
        }

        [Fact]
        public void Quantity_Division_WithNormalDistribution_CombinesCorrectly()
        {
            UncertaintyQuantity<Length> a = Length.FromMeters(60).WithNormalDistributionUncertainty(3);
            double divisor = 3.0;

            UncertaintyQuantity<Length> result = a.Divide(divisor);

            Assert.Equal(20.0, result.CentralValue.Meters, 6);
            Assert.InRange(result.LowerBound.Meters, 17.0, 17.1);
            Assert.InRange(result.UpperBound.Meters, 23.0, 23.1);

        }
    }
}
