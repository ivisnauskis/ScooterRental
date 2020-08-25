using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using Xunit;

namespace ScooterRental.tests
{
    public class RideServiceTests
    {
        private IRideService _service;
        private readonly Mock<IRentCalculator> _calculator;

        public RideServiceTests()
        {
            _calculator = new Mock<IRentCalculator>();
            _calculator.Setup(x =>
                x.CalculateRentalPrice(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0.2M)).Returns(2);
            _service = new RideService(_calculator.Object);
        }

        [Fact]
        public void StartRide()
        {
            Scooter scooter = new Scooter("1", 0.2M);
            _service.StartRide(scooter);

            scooter.IsRented.Should().Be(true);
        }

        [Fact]
        public void StartRent_ScooterAlreadyRented_ShouldThrowScooterRentalInProgressException()
        {
            Scooter scooter = new Scooter("1", 0.2M);
            _service.StartRide(scooter);

            Action act = () => _service.StartRide(scooter);
            act.Should().Throw<ScooterRentalInProgressException>();
        }

        [Fact]
        public void EndRide()
        {
            var scooter = new Scooter("1", 0.2M);
            var ride = new Ride(scooter, DateTime.Now);
            _service.StartRide(scooter);

            _service.EndRide(ride.Scooter.Id);

            scooter.IsRented.Should().Be(false);
        }

        [Fact]
        public void EndRide_ScooterNotRented_ShouldThrowScooterNotRentedException()
        {
            var scooter = new Scooter("1", 0.2M);
            var ride = new Ride(scooter, DateTime.Now);

            Action act = () => _service.EndRide(ride.Scooter.Id);
            act.Should().Throw<ScooterNotRentedException>();
        }

        [Fact]
        public void GetRideHistory()
        {
            _service.StartRide(new Scooter("1", 0.2M));
            _service.EndRide("1");

            _service.GetRideHistory(null).Count.Should().Be(1);
        }


        [Theory]
        [InlineData(null, 8)]
        [InlineData(2018, 2)]
        [InlineData(2020, 6)]
        public void GetActiveRidesPrice(int? year, decimal expectedPrice)
        {
            _service = new RideService(_calculator.Object, GetActiveRides(), null);
            _service.GetActiveRidesPrice(year).Should().Be(expectedPrice);
        }

        private Dictionary<string, Ride> GetActiveRides()
        {
            return new Dictionary<string, Ride>
            {
                {"7", new Ride(new Scooter("7", 0.2M), new DateTime(2020, 8, 1))},
                {"8", new Ride(new Scooter("8", 0.2M), new DateTime(2020, 8, 1))},
                {"9", new Ride(new Scooter("9", 0.2M), new DateTime(2020, 8, 1))},
                {"10", new Ride(new Scooter("10", 0.2M), new DateTime(2018, 8, 1))}
            };
        }
    }
}