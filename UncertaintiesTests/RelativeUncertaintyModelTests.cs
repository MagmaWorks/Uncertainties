﻿namespace MagmaWorks.Uncertainties.Tests
{
    public class RelativeUncertaintyModelTests
    {
        [Fact]
        public void Scalar_WithRelativeUncertainty_HasCorrectBounds()
        {
            UncertaintyScalar<double> value = 200.0.WithRelativeUncertainty(0.1); // ±10%

            Assert.Equal(200.0, value.CentralValue);
            Assert.Equal(180.0, value.LowerBound, 6);
            Assert.Equal(220.0, value.UpperBound, 6);
        }

        [Fact]
        public void Quantity_WithRelativeUncertainty_HasCorrectBounds()
        {
            UncertaintyQuantity<Pressure> pressure = Pressure.FromPascals(1000).WithRelativeUncertainty(0.25); // ±25%

            Assert.Equal(750, pressure.LowerBound.Pascals, 6);
            Assert.Equal(1250, pressure.UpperBound.Pascals, 6);
        }

        [Fact]
        public void Scalar_Addition_WithRelativeUncertainty_CombinesCorrectly()
        {
            var a = 200.0.WithRelativeUncertainty(0.1); // ±20
            var b = 100.0.WithRelativeUncertainty(0.2); // ±20

            var result = a.Add(b);

            Assert.Equal(300.0, result.CentralValue, 6);
            Assert.Equal(260.0, result.LowerBound, 6);
            Assert.Equal(340.0, result.UpperBound, 6);
        }

        [Fact]
        public void Quantity_Addition_WithRelativeUncertainty_CombinesCorrectly()
        {
            var a = Mass.FromKilograms(80).WithRelativeUncertainty(0.1);  // ±8
            var b = Mass.FromKilograms(40).WithRelativeUncertainty(0.2);  // ±8

            var result = a.Add(b);

            Assert.Equal(120.0, result.CentralValue.Kilograms, 6);
            Assert.Equal(104.0, result.LowerBound.Kilograms, 6);
            Assert.Equal(136.0, result.UpperBound.Kilograms, 6);
        }

        [Fact]
        public void Scalar_Subtraction_WithRelativeUncertainty_CombinesCorrectly()
        {
            var a = 200.0.WithRelativeUncertainty(0.1); // ±20 → 180 to 220
            var b = 100.0.WithRelativeUncertainty(0.2); // ±20 → 80 to 120

            var result = a.Subtract(b);

            Assert.Equal(100.0, result.CentralValue, 6);
            Assert.Equal(60.0, result.LowerBound, 6);
            Assert.Equal(140.0, result.UpperBound, 6);
        }

        [Fact]
        public void Quantity_Subtraction_WithRelativeUncertainty_CombinesCorrectly()
        {
            var a = Speed.FromKilometersPerHour(90).WithRelativeUncertainty(0.1); // ±9
            var b = Speed.FromKilometersPerHour(30).WithRelativeUncertainty(0.2); // ±6

            var result = a.Subtract(b);

            Assert.Equal(60.0, result.CentralValue.KilometersPerHour, 6);
            Assert.Equal(45.0, result.LowerBound.KilometersPerHour, 6);
            Assert.Equal(75.0, result.UpperBound.KilometersPerHour, 6);
        }

        [Fact]
        public void Scalar_Multiplication_WithRelativeUncertainty_CombinesCorrectly()
        {
            var a = 100.0.WithRelativeUncertainty(0.1); // 10%
            var factor = 2.0;

            var result = a.Multiply(factor);

            Assert.Equal(200.0, result.CentralValue, 6);
            Assert.Equal(180.0, result.LowerBound, 6);  // 200 * (1 - 0.1)
            Assert.Equal(220.0, result.UpperBound, 6);  // 200 * (1 + 0.1)
        }

        [Fact]
        public void Scalar_Division_WithRelativeUncertainty_CombinesCorrectly()
        {
            var a = 100.0.WithRelativeUncertainty(0.2); // 20%
            var divisor = 2.0;

            var result = a.Divide(divisor);

            Assert.Equal(50.0, result.CentralValue, 6);
            Assert.Equal(40.0, result.LowerBound, 6);  // 50 * (1 - 0.2)
            Assert.Equal(60.0, result.UpperBound, 6);  // 50 * (1 + 0.2)
        }

        [Fact]
        public void Quantity_Multiplication_WithRelativeUncertainty_CombinesCorrectly()
        {
            var a = Length.FromMeters(100).WithRelativeUncertainty(0.05); // 5%
            var factor = 4.0;

            var result = a.Multiply(factor);

            Assert.Equal(400.0, result.CentralValue.Meters, 6);
            Assert.Equal(380.0, result.LowerBound.Meters, 6);
            Assert.Equal(420.0, result.UpperBound.Meters, 6);
        }

        [Fact]
        public void Quantity_Division_WithRelativeUncertainty_CombinesCorrectly()
        {
            var a = Length.FromMeters(50).WithRelativeUncertainty(0.1); // 10%
            var divisor = 2.0;

            var result = a.Divide(divisor);

            Assert.Equal(25.0, result.CentralValue.Meters, 6);
            Assert.Equal(22.5, result.LowerBound.Meters, 6);
            Assert.Equal(27.5, result.UpperBound.Meters, 6);
        }
    }
}
