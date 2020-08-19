﻿using System;

namespace ScooterRental
{
    public class Ride
    {
        public Ride(Scooter scooter, DateTime startTime)
        {
            Scooter = scooter;
            StartTime = startTime;
            IsActive = true;
        }

        public Scooter Scooter { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; private set; }
        public decimal RidePrice { get; private set; }
        public bool IsActive { get; private set; }

        public void EndRide(DateTime endTime, decimal ridePrice)
        {
            EndTime = endTime;
            RidePrice = ridePrice;
            IsActive = false;
        }
    }
}