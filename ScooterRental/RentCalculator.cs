using System;

namespace ScooterRental
{
    public class RentCalculator : IRentCalculator
    {
        private readonly decimal _maxPrice;

        public RentCalculator(decimal maxPrice)
        {
            _maxPrice = maxPrice;
        }

        public decimal CalculateRentalPrice(DateTime start, DateTime end, decimal price)
        {
            return (end.Date - start.Date).TotalDays > 1
                ? CalculateMultiDayRental(start, end, price)
                : CalculateSingleDayRental(start, end, price);
        }

        private decimal CalculateSingleDayRental(DateTime start, DateTime end, decimal price)
        {
            var rentalPrice = (decimal) (end - start).TotalMinutes * price;
            return rentalPrice > _maxPrice ? _maxPrice : rentalPrice;
        }

        private decimal CalculateMultiDayRental(DateTime start, DateTime end, decimal price)
        {
            var firstDayPrice = GetFirstDayPrice(start, price);
            var lastDayPrice = GetLastDayPrice(end, price);
            var BetweenDaysPrice = GetFullDayCount(start, end) * _maxPrice;

            return firstDayPrice + BetweenDaysPrice + lastDayPrice;
        }

        private decimal GetFullDayCount(DateTime start, DateTime end)
        {
            return (decimal)(end.Date - start.Date).TotalDays - 1;
        }

        private decimal GetFirstDayPrice(DateTime start, decimal price)
        {
            var firstDayPrice = (decimal) (24 * 60 - start.TimeOfDay.TotalMinutes) * price;
            return firstDayPrice >= _maxPrice ? _maxPrice : 0M;
        }

        private decimal GetLastDayPrice(DateTime end, decimal price)
        {
            var secondDayPrice = (decimal) end.TimeOfDay.TotalMinutes * price;
            return secondDayPrice > _maxPrice ? _maxPrice : secondDayPrice;
        }
    }
}