using System;
using System.Collections.Generic;

namespace ScooterRental.Interfaces
{
    public interface IRentCalculator
    {
        decimal CalculateRentalPrice(DateTime start, DateTime end, decimal pricePerMinute);

        decimal CalculateIncome(IList<Ride> rideHistory);
    }
}