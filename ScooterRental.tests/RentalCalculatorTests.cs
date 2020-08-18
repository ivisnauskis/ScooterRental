using System;
using Xunit;

namespace ScooterRental.tests
{
    public class RentalCalculatorTests
    {
        private readonly IRentCalculator accountant;
        private readonly decimal maxPrice = 20;

        public RentalCalculatorTests()
        {
            accountant = new RentCalculator(maxPrice);
        }

        [Fact]
        public void CalculateRentalPrice_NotExceedingMaxSameDay_ShouldReturnPrice()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var stop = new DateTime(2020, 1, 1, 12, 10, 0);
            var price = 0.2M;

            Assert.Equal(2M, accountant.CalculateRentalPrice(start, stop, price));
        }

        [Fact]
        public void CalculateRentalPrice_ExceedingMaxSumSameDay_ShouldReturnPrice()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var stop = new DateTime(2020, 1, 1, 14, 0, 0);
            var price = 0.2M;

            Assert.Equal(20M, accountant.CalculateRentalPrice(start, stop, price));
        }

        [Fact]
        public void CalculateRentalPrice_NotExceedingSumDifferentDay_ShouldReturnSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 2, 0, 10, 0);
            var price = 0.2M;

            Assert.Equal(4M, accountant.CalculateRentalPrice(start, end, price));
        }

        [Fact]
        public void CalculateRentalPrice_24h_ShouldReturnMax()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 2, 23, 50, 0);
            var price = 0.2M;

            Assert.Equal(20M, accountant.CalculateRentalPrice(start, end, price));
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysFirstAndLastNotMax_ShouldReturnMaxPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 3, 0, 10, 0);
            var price = 0.2M;

            Assert.Equal(22, accountant.CalculateRentalPrice(start, end, price));
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysFirstDayMax_ShouldReturnMaxTimesDaysPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var end = new DateTime(2020, 1, 3, 0, 10, 0);
            var price = 0.2M;
            var result = accountant.CalculateRentalPrice(start, end, price);
            Assert.Equal(42, result);
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysLastMax_ShouldReturnMaxTimesDaysPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 3, 3, 00, 0);
            var price = 0.2M;
            var result = accountant.CalculateRentalPrice(start, end, price);
            Assert.Equal(40, result);
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysFirstMaxLastMax_ShouldReturnThreeTimesMax()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var end = new DateTime(2020, 1, 3, 12, 0, 0);
            var price = 0.2M;
            var result = accountant.CalculateRentalPrice(start, end, price);
            Assert.Equal(60, result);
        }

        [Fact]
        public void CalculateRentalPrice_FourDaysNoneMax_ShouldReturnThreeTimesMaxPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 0, 0);
            var end = new DateTime(2020, 1, 4, 1, 0, 0);
            var price = 0.2M;
            var result = accountant.CalculateRentalPrice(start, end, price);
            Assert.Equal(52, result);
        }

        [Fact]
        public void CalculateRentalPrice_FourDaysFirstMaxLastMax_ShouldReturnFourTimesMax()
        {
            var start = new DateTime(2020, 1, 1, 22, 0, 0);
            var end = new DateTime(2020, 1, 4, 2, 0, 0);
            var price = 0.2M;
            var result = accountant.CalculateRentalPrice(start, end, price);
            Assert.Equal(80, result);
        }

        [Fact]
        public void CalculateRentalPrice_NextMonth()
        {
            var start = new DateTime(2020, 8, 31, 23, 50, 0);
            var end = new DateTime(2020, 9, 1, 0, 10, 0);
            var price = 0.2M;
            var result = accountant.CalculateRentalPrice(start, end, price);
            Assert.Equal(4, result);
        }

        [Fact]
        public void CalculateRentalPrice_NextYear()
        {
            var start = new DateTime(2020, 12, 31, 23, 50, 0);
            var end = new DateTime(2021, 1, 1, 0, 10, 0);
            var price = 0.2M;
            var result = accountant.CalculateRentalPrice(start, end, price);
            Assert.Equal(4, result);
        }

        [Fact]
        public void CalculateRentalPrice_NextMonthThreeDays()
        {
            var start = new DateTime(2020, 11, 30, 23, 50, 0);
            var end = new DateTime(2020, 12, 2, 0, 10, 0);
            var price = 0.2M;
            var result = accountant.CalculateRentalPrice(start, end, price);
            Assert.Equal(22, result);
        }

        [Fact]
        public void CalculateRentalPrice_NextYearThreeDays()
        {
            var start = new DateTime(2020, 12, 31, 23, 50, 0);
            var end = new DateTime(2021, 1, 2, 0, 10, 0);
            var price = 0.2M;
            var result = accountant.CalculateRentalPrice(start, end, price);
            Assert.Equal(22, result);
        }
    }
}