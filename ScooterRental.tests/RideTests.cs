using System;
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

            Assert.Equal("1", ride.Scooter.Id);
            Assert.Equal(new DateTime(2019, 1, 1, 12, 0, 0), ride.RideStartTime);
            Assert.Equal(new DateTime(2019, 1, 1, 12, 10, 0), ride.RideEndTime);
            Assert.Equal(1M, ride.RidePrice);
            Assert.False(ride.IsActive);
        }
    }
}