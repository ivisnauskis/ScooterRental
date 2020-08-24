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

        [Theory]
        [InlineData("01/01/2020 12:00:00", "01/01/2020 12:10:00", 2)] // Same day, not exceeding max price
        [InlineData("01/01/2020 12:00:00", "01/01/2020 14:00:00", 20)] // Same day, exceeding max price
        [InlineData("01/01/2020 23:50:00", "02/01/2020 00:10:00", 4)] // Next day, not exceeding max price
        [InlineData("01/01/2020 23:50:00", "02/01/2020 23:50:00", 20)] // Next day, exceeding max price
        [InlineData("01/01/2020 23:50:00", "03/01/2020 00:10:00", 22)] // Three days, not exceeding, full, not exceeding
        [InlineData("01/01/2020 12:00:00", "03/01/2020 00:10:00", 42)] // Three days, exceeding, full, not exceeding
        [InlineData("01/01/2020 23:50:00", "03/01/2020 03:00:00", 40)] // Three days, not exceeding, full, exceeding
        [InlineData("01/01/2020 12:00:00", "03/01/2020 12:00:00", 60)] // three days, all exceeding
        [InlineData("01/01/2020 23:00:00", "04/01/2020 01:00:00", 52)] // four days
        [InlineData("01/01/2020 22:00:00", "04/01/2020 02:00:00", 80)] // four days, all exceeding
        [InlineData("31/01/2020 23:50:00", "01/02/2020 00:10:00", 4)] // Next month
        [InlineData("31/12/2020 23:50:00", "01/01/2021 00:10:00", 4)] // Next year
        [InlineData("30/11/2020 23:50:00", "02/12/2020 00:10:00", 22)] // Next month, three days
        [InlineData("31/12/2020 23:50:00", "02/01/2021 00:10:00", 22)] // Next year, three days
        public void CalculateRentalPrice(string startString, string stopString, decimal expectedPrice)
        {
            var start = DateTime.Parse(startString);
            var stop = DateTime.Parse(stopString);
            _calculator.CalculateRentalPrice(start, stop, 0.2M).Should().Be(expectedPrice);
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