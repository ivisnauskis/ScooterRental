using System;
using FluentAssertions;
using Xunit;

namespace ScooterRental.tests
{
    public class RideTests
    {
        [Fact]
        public void RideTest()
        {
            var ride = new Ride(new Scooter("1", 0.2M), new DateTime(2019, 1, 1, 12, 0, 0));
            ride.EndRide(new DateTime(2019, 1, 1, 12, 10, 0), 1M);

            ride.Scooter.Id.Should().Be("1");
            ride.StartTime.Should().Be(new DateTime(2019, 1, 1, 12, 0, 0));
            ride.EndTime.Should().Be(new DateTime(2019, 1, 1, 12, 10, 0));
            ride.RidePrice.Should().Be(1M);
        }
    }
}