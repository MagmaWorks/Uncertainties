namespace MagmaWorks.Uncertainties.Tests
{
    public class IntervalUncertaintyModelTests
    {
        [Fact]
        public void Scalar_WithInterval_HasCorrectBounds()
        {
            Uncertainty value = 50.0.WithIntervalUncertainty(45.0, 55.0);

            Assert.Equal(45.0, value.LowerBound, 6);
            Assert.Equal(55.0, value.UpperBound, 6);
            Assert.Equal(50.0, value.CentralValue);
        }

        [Fact]
        public void Quantity_WithInterval_HasCorrectBounds()
        {
            UncertaintyQuantity<Duration> time = Duration.FromSeconds(60).WithIntervalUncertainty(58.0, 63.0);

            Assert.Equal(58.0, time.LowerBound.Seconds, 6);
            Assert.Equal(63.0, time.UpperBound.Seconds, 6);
            Assert.Equal(60.0, time.CentralValue.Seconds, 6);
        }

        [Fact]
        public void Interval_WithSwappedBounds_Reverts()
        {
            Uncertainty interv = 20.0.WithIntervalUncertainty(30.0, 10.0);

            Assert.Equal(10.0, interv.LowerBound, 6);
            Assert.Equal(30.0, interv.UpperBound, 6);
        }

        [Fact]
        public void Scalar_Addition_WithIntervalUncertainty_CombinesCorrectly()
        {
            Uncertainty a = 50.0.WithIntervalUncertainty(40.0, 60.0);
            Uncertainty b = 20.0.WithIntervalUncertainty(18.0, 22.0);

            Uncertainty result = a.Add(b);

            Assert.Equal(70.0, result.CentralValue, 6);
            Assert.Equal(58.0, result.LowerBound, 6);
            Assert.Equal(82.0, result.UpperBound, 6);
        }

        [Fact]
        public void Quantity_Addition_WithIntervalUncertainty_CombinesCorrectly()
        {
            UncertaintyQuantity<Volume> a = Volume.FromLiters(100).WithIntervalUncertainty(90, 110);
            UncertaintyQuantity<Volume> b = Volume.FromLiters(50).WithIntervalUncertainty(40, 60);

            UncertaintyQuantity<Volume> result = a.Add(b);

            Assert.Equal(150.0, result.CentralValue.Liters, 6);
            Assert.Equal(130.0, result.LowerBound.Liters, 6);
            Assert.Equal(170.0, result.UpperBound.Liters, 6);
        }

        [Fact]
        public void Scalar_Subtraction_WithIntervalUncertainty_CombinesCorrectly()
        {
            Uncertainty a = 100.0.WithIntervalUncertainty(90.0, 110.0);
            Uncertainty b = 60.0.WithIntervalUncertainty(50.0, 70.0);

            Uncertainty result = a.Subtract(b);

            Assert.Equal(40.0, result.CentralValue, 6);
            Assert.Equal(20.0, result.LowerBound, 6);
            Assert.Equal(60.0, result.UpperBound, 6);
        }

        [Fact]
        public void Quantity_Subtraction_WithIntervalUncertainty_CombinesCorrectly()
        {
            UncertaintyQuantity<Temperature> a = Temperature.FromDegreesCelsius(20).WithIntervalUncertainty(18, 22);
            UncertaintyQuantity<Temperature> b = Temperature.FromDegreesCelsius(5).WithIntervalUncertainty(3, 8);

            UncertaintyQuantity<Temperature> result = a.Subtract(b);

            Assert.Equal(15, result.CentralValue.DegreesCelsius, 6);
            Assert.Equal(10, result.LowerBound.DegreesCelsius, 6);
            Assert.Equal(19, result.UpperBound.DegreesCelsius, 6);
        }

        [Fact]
        public void Scalar_Multiplication_WithIntervalUncertainty_CombinesCorrectly()
        {
            Uncertainty a = 100.0.WithIntervalUncertainty(90, 110);
            double factor = 2.0;

            Uncertainty result = a.Multiply(factor);

            Assert.Equal(200.0, result.CentralValue, 6);
            Assert.Equal(180.0, result.LowerBound, 6);
            Assert.Equal(220.0, result.UpperBound, 6);
        }

        [Fact]
        public void Scalar_Division_WithIntervalUncertainty_CombinesCorrectly()
        {
            Uncertainty a = 100.0.WithIntervalUncertainty(80, 120);
            double divisor = 2.0;

            Uncertainty result = a.Divide(divisor);

            Assert.Equal(50.0, result.CentralValue, 6);
            Assert.Equal(40.0, result.LowerBound, 6);
            Assert.Equal(60.0, result.UpperBound, 6);
        }

        [Fact]
        public void Quantity_Multiplication_WithIntervalUncertainty_CombinesCorrectly()
        {
            UncertaintyQuantity<Length> a = Length.FromMeters(100).WithIntervalUncertainty(90, 110);
            double factor = 3.0;

            UncertaintyQuantity<Length> result = a.Multiply(factor);

            Assert.Equal(300.0, result.CentralValue.Meters, 6);
            Assert.Equal(270.0, result.LowerBound.Meters, 6);
            Assert.Equal(330.0, result.UpperBound.Meters, 6);
        }

        [Fact]
        public void Quantity_Division_WithIntervalUncertainty_CombinesCorrectly()
        {
            UncertaintyQuantity<Length> a = Length.FromMeters(150).WithIntervalUncertainty(120, 180);
            double divisor = 3.0;

            UncertaintyQuantity<Length> result = a.Divide(divisor);

            Assert.Equal(50.0, result.CentralValue.Meters, 6);
            Assert.Equal(40.0, result.LowerBound.Meters, 6);
            Assert.Equal(60.0, result.UpperBound.Meters, 6);
        }
    }
}
