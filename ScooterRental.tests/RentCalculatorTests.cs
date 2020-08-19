using System;
using System.Collections.Generic;
using ScooterRental.Exceptions;
using Xunit;

namespace ScooterRental.tests
{
    public class RentCalculatorTests
    {
        private readonly IRentCalculator _calculator;
        private readonly decimal maxPrice = 20;

        public RentCalculatorTests()
        {
            _calculator = new RentCalculator(maxPrice);
        }

        [Fact]
        public void CalculateRentalPrice_StartGreaterThanEnd_ShouldThrowException()
        {
            var start = new DateTime(2020, 1, 2, 12, 0, 0);
            var stop = new DateTime(2020, 1, 1, 12, 10, 0);
            var price = 0.2M;

            Assert.Throws<IncorrectTimeException>(() => _calculator.CalculateRentalPrice(start, stop, price));
        }

        [Fact]
        public void CalculateRentalPrice_NotExceedingMaxSameDay_ShouldReturnPrice()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var stop = new DateTime(2020, 1, 1, 12, 10, 0);
            var price = 0.2M;

            Assert.Equal(2M, _calculator.CalculateRentalPrice(start, stop, price));
        }

        [Fact]
        public void CalculateRentalPrice_ExceedingMaxSumSameDay_ShouldReturnPrice()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var stop = new DateTime(2020, 1, 1, 14, 0, 0);
            var price = 0.2M;

            Assert.Equal(20M, _calculator.CalculateRentalPrice(start, stop, price));
        }

        [Fact]
        public void CalculateRentalPrice_NotExceedingSumDifferentDay_ShouldReturnSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 2, 0, 10, 0);
            var price = 0.2M;

            Assert.Equal(4M, _calculator.CalculateRentalPrice(start, end, price));
        }

        [Fact]
        public void CalculateRentalPrice_24h_ShouldReturnMax()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 2, 23, 50, 0);
            var price = 0.2M;

            Assert.Equal(20M, _calculator.CalculateRentalPrice(start, end, price));
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysFirstAndLastNotMax_ShouldReturnMaxPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 3, 0, 10, 0);
            var price = 0.2M;

            Assert.Equal(22, _calculator.CalculateRentalPrice(start, end, price));
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysFirstDayMax_ShouldReturnMaxTimesDaysPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var end = new DateTime(2020, 1, 3, 0, 10, 0);
            var price = 0.2M;
            var result = _calculator.CalculateRentalPrice(start, end, price);
            Assert.Equal(42, result);
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysLastMax_ShouldReturnMaxTimesDaysPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 3, 3, 00, 0);
            var price = 0.2M;
            var result = _calculator.CalculateRentalPrice(start, end, price);
            Assert.Equal(40, result);
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysFirstMaxLastMax_ShouldReturnThreeTimesMax()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var end = new DateTime(2020, 1, 3, 12, 0, 0);
            var price = 0.2M;
            var result = _calculator.CalculateRentalPrice(start, end, price);
            Assert.Equal(60, result);
        }

        [Fact]
        public void CalculateRentalPrice_FourDaysNoneMax_ShouldReturnThreeTimesMaxPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 0, 0);
            var end = new DateTime(2020, 1, 4, 1, 0, 0);
            var price = 0.2M;
            var result = _calculator.CalculateRentalPrice(start, end, price);
            Assert.Equal(52, result);
        }

        [Fact]
        public void CalculateRentalPrice_FourDaysFirstMaxLastMax_ShouldReturnFourTimesMax()
        {
            var start = new DateTime(2020, 1, 1, 22, 0, 0);
            var end = new DateTime(2020, 1, 4, 2, 0, 0);
            var price = 0.2M;
            var result = _calculator.CalculateRentalPrice(start, end, price);
            Assert.Equal(80, result);
        }

        [Fact]
        public void CalculateRentalPrice_NextMonth()
        {
            var start = new DateTime(2020, 8, 31, 23, 50, 0);
            var end = new DateTime(2020, 9, 1, 0, 10, 0);
            var price = 0.2M;
            var result = _calculator.CalculateRentalPrice(start, end, price);
            Assert.Equal(4, result);
        }

        [Fact]
        public void CalculateRentalPrice_NextYear()
        {
            var start = new DateTime(2020, 12, 31, 23, 50, 0);
            var end = new DateTime(2021, 1, 1, 0, 10, 0);
            var price = 0.2M;
            var result = _calculator.CalculateRentalPrice(start, end, price);
            Assert.Equal(4, result);
        }

        [Fact]
        public void CalculateRentalPrice_NextMonthThreeDays()
        {
            var start = new DateTime(2020, 11, 30, 23, 50, 0);
            var end = new DateTime(2020, 12, 2, 0, 10, 0);
            var price = 0.2M;
            var result = _calculator.CalculateRentalPrice(start, end, price);
            Assert.Equal(22, result);
        }

        [Fact]
        public void CalculateRentalPrice_NextYearThreeDays()
        {
            var start = new DateTime(2020, 12, 31, 23, 50, 0);
            var end = new DateTime(2021, 1, 2, 0, 10, 0);
            var price = 0.2M;
            var result = _calculator.CalculateRentalPrice(start, end, price);
            Assert.Equal(22, result);
        }

        [Fact]
        public void CalculateIncome()
        {
            var ride1 = new Ride(new Scooter("1", 0.2M), new DateTime(2019, 1, 1));
            ride1.EndRide(new DateTime(2019, 1, 1), 14M);
            var ride2 = new Ride(new Scooter("2", 0.2M), new DateTime(2019, 1, 1));
            ride2.EndRide(new DateTime(2019, 1, 1), 15M);
            var ride3 = new Ride(new Scooter("3", 0.2M), new DateTime(2019, 1, 1));
            ride3.EndRide(new DateTime(2019, 1, 1), 16M);

            var list = new List<Ride> {ride1, ride2, ride3};
            var income = _calculator.CalculateIncome(list);

            Assert.Equal(45, income);
        }
    }
}