using System;
using System.Collections.Generic;

namespace ScooterRental
{
    public interface IRentCalculator
    {
        decimal CalculateRentalPrice(DateTime start, DateTime end, decimal pricePerMinute);

        decimal CalculateIncome(List<Ride> rideHistory);
    }
}