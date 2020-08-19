using System;

namespace ScooterRental
{
    public class Ride
    {
        public Scooter Scooter { get; }
        public DateTime RideStartTime { get; }
        public DateTime RideEndTime { get; private set; }
        public decimal RidePrice { get; private set; }
        public bool IsActive { get; private set; }

        public Ride(Scooter scooter, DateTime rideStartTime)
        {
            Scooter = scooter;
            RideStartTime = rideStartTime;
            IsActive = true;
        }

        public void EndRide(DateTime endTime, decimal ridePrice)
        {
            RideEndTime = endTime;
            RidePrice = ridePrice;
            IsActive = false;
        }
    }
}