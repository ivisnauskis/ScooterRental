using System;

namespace ScooterRental
{
    public interface IRentCalculator
    {
        decimal CalculateRentalPrice(DateTime start, DateTime end, decimal pricePerMinute);
    }
}