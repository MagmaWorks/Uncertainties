using UnitsNet;

namespace MagmaWorks.Uncertainties.Tests
{
    public class AbsoluteUncertaintyModelTests
    {
        [Fact]
        public void Scalar_WithAbsoluteUncertainty_HasCorrectBounds()
        {
            UncertaintyScalar<double> value = 100.0.WithAbsoluteUncertainty(15.0);

            Assert.Equal(100.0, value.CentralValue);
            Assert.Equal(85.0, value.LowerBound, 6);
            Assert.Equal(115.0, value.UpperBound, 6);
        }

        [Fact]
        public void Quantity_WithAbsoluteUncertainty_HasCorrectBounds()
        {
            UncertaintyQuantity<Length> length = Length.FromMeters(2.5).WithAbsoluteUncertainty(0.1);

            Assert.Equal(2.5, length.CentralValue.Meters, 6);
            Assert.Equal(2.4, length.LowerBound.Meters, 6);
            Assert.Equal(2.6, length.UpperBound.Meters, 6);
        }

        [Fact]
        public void Scalar_Addition_WithAbsoluteUncertainty_CombinesCorrectly()
        {
            UncertaintyScalar<double> a = 100.0.WithAbsoluteUncertainty(5);
            UncertaintyScalar<double> b = 50.0.WithAbsoluteUncertainty(2.5);

            UncertaintyScalar<double> result = a.Add(b);

            Assert.Equal(150.0, result.CentralValue, 6);
            Assert.Equal(142.5, result.LowerBound, 6);
            Assert.Equal(157.5, result.UpperBound, 6);
        }

        [Fact]
        public void Quantity_Addition_WithAbsoluteUncertainty_CombinesCorrectly()
        {
            UncertaintyQuantity<Length> a = Length.FromMeters(100.0).WithAbsoluteUncertainty(5);
            UncertaintyQuantity<Length> b = Length.FromMeters(50.0).WithAbsoluteUncertainty(2.5);

            UncertaintyQuantity<Length> result = a.Add(b);

            Assert.Equal(150.0, result.CentralValue.Meters, 6);
            Assert.Equal(142.5, result.LowerBound.Meters, 6);
            Assert.Equal(157.5, result.UpperBound.Meters, 6);
        }

        [Fact]
        public void Scalar_Subtraction_WithAbsoluteUncertainty_CombinesCorrectly()
        {
            UncertaintyScalar<double> a = 100.0.WithAbsoluteUncertainty(5);
            UncertaintyScalar<double> b = 50.0.WithAbsoluteUncertainty(2.5);

            UncertaintyScalar<double> result = a.Subtract(b);

            Assert.Equal(50.0, result.CentralValue, 6);
            Assert.Equal(42.5, result.LowerBound, 6);
            Assert.Equal(57.5, result.UpperBound, 6);
        }

        [Fact]
        public void Quantity_Subtraction_WithAbsoluteUncertainty_CombinesCorrectly()
        {
            UncertaintyQuantity<Length> a = Length.FromMeters(100.0).WithAbsoluteUncertainty(5);
            UncertaintyQuantity<Length> b = Length.FromMeters(50.0).WithAbsoluteUncertainty(2.5);

            UncertaintyQuantity<Length> result = a.Subtract(b);

            Assert.Equal(50.0, result.CentralValue.Meters, 6);
            Assert.Equal(42.5, result.LowerBound.Meters, 6);
            Assert.Equal(57.5, result.UpperBound.Meters, 6);
        }

        [Fact]
        public void Scalar_Multiplication_WithAbsoluteUncertainty_CombinesCorrectly()
        {
            var a = 100.0.WithAbsoluteUncertainty(5);
            var factor = 2.0;

            var result = a.Multiply(factor);

            Assert.Equal(200.0, result.CentralValue, 6);
            Assert.Equal(190.0, result.LowerBound, 6);  // 200 - 10
            Assert.Equal(210.0, result.UpperBound, 6); // 200 + 10
        }

        [Fact]
        public void Scalar_Division_WithAbsoluteUncertainty_CombinesCorrectly()
        {
            var a = 100.0.WithAbsoluteUncertainty(10);
            var divisor = 2.0;

            var result = a.Divide(divisor);

            Assert.Equal(50.0, result.CentralValue, 6);
            Assert.Equal(45.0, result.LowerBound, 6);  // 50 - 5
            Assert.Equal(55.0, result.UpperBound, 6);  // 50 + 5
        }

        [Fact]
        public void Quantity_Multiplication_WithAbsoluteUncertainty_CombinesCorrectly()
        {
            var a = Length.FromMeters(100).WithAbsoluteUncertainty(5);
            var factor = 3.0;

            var result = a.Multiply(factor);

            Assert.Equal(300.0, result.CentralValue.Meters, 6);
            Assert.Equal(285.0, result.LowerBound.Meters, 6);
            Assert.Equal(315.0, result.UpperBound.Meters, 6);
        }

        [Fact]
        public void Quantity_Division_WithAbsoluteUncertainty_CombinesCorrectly()
        {
            var a = Length.FromMeters(150).WithAbsoluteUncertainty(15);
            var divisor = 3.0;

            var result = a.Divide(divisor);

            Assert.Equal(50.0, result.CentralValue.Meters, 6);
            Assert.Equal(45.0, result.LowerBound.Meters, 6);
            Assert.Equal(55.0, result.UpperBound.Meters, 6);
        }
    }
}
