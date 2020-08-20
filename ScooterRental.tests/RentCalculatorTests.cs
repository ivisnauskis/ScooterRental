using System;
using System.Collections.Generic;
using FluentAssertions;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
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

            Action asd = () => _calculator.CalculateRentalPrice(start, stop, price);
            asd.Should().Throw<IncorrectTimeException>();
        }

        [Fact]
        public void CalculateRentalPrice_NotExceedingMaxSameDay_ShouldReturnPrice()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var stop = new DateTime(2020, 1, 1, 12, 10, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, stop, price).Should().Be(2M);
        }

        [Fact]
        public void CalculateRentalPrice_ExceedingMaxSumSameDay_ShouldReturnPrice()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var stop = new DateTime(2020, 1, 1, 14, 0, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, stop, price).Should().Be(20M);
        }

        [Fact]
        public void CalculateRentalPrice_NotExceedingSumDifferentDay_ShouldReturnSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 2, 0, 10, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(4M);
        }

        [Fact]
        public void CalculateRentalPrice_24h_ShouldReturnMax()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 2, 23, 50, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(20M);
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysFirstAndLastNotMax_ShouldReturnMaxPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 3, 0, 10, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(22);
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysFirstDayMax_ShouldReturnMaxTimesDaysPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var end = new DateTime(2020, 1, 3, 0, 10, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(42);
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysLastMax_ShouldReturnMaxTimesDaysPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 50, 0);
            var end = new DateTime(2020, 1, 3, 3, 00, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(40M);
        }

        [Fact]
        public void CalculateRentalPrice_ThreeDaysFirstMaxLastMax_ShouldReturnThreeTimesMax()
        {
            var start = new DateTime(2020, 1, 1, 12, 0, 0);
            var end = new DateTime(2020, 1, 3, 12, 0, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(60);
        }

        [Fact]
        public void CalculateRentalPrice_FourDaysNoneMax_ShouldReturnThreeTimesMaxPlusSum()
        {
            var start = new DateTime(2020, 1, 1, 23, 0, 0);
            var end = new DateTime(2020, 1, 4, 1, 0, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(52);
        }

        [Fact]
        public void CalculateRentalPrice_FourDaysFirstMaxLastMax_ShouldReturnFourTimesMax()
        {
            var start = new DateTime(2020, 1, 1, 22, 0, 0);
            var end = new DateTime(2020, 1, 4, 2, 0, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(80);
        }

        [Fact]
        public void CalculateRentalPrice_NextMonth()
        {
            var start = new DateTime(2020, 8, 31, 23, 50, 0);
            var end = new DateTime(2020, 9, 1, 0, 10, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(4);
        }

        [Fact]
        public void CalculateRentalPrice_NextYear()
        {
            var start = new DateTime(2020, 12, 31, 23, 50, 0);
            var end = new DateTime(2021, 1, 1, 0, 10, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(4);
        }

        [Fact]
        public void CalculateRentalPrice_NextMonthThreeDays()
        {
            var start = new DateTime(2020, 11, 30, 23, 50, 0);
            var end = new DateTime(2020, 12, 2, 0, 10, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(22);
        }

        [Fact]
        public void CalculateRentalPrice_NextYearThreeDays()
        {
            var start = new DateTime(2020, 12, 31, 23, 50, 0);
            var end = new DateTime(2021, 1, 2, 0, 10, 0);
            var price = 0.2M;

            _calculator.CalculateRentalPrice(start, end, price).Should().Be(22);
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

            _calculator.CalculateIncome(list).Should().Be(45);
        }
    }
}