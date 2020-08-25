using System;
using System.Collections.Generic;
using System.Linq;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental
{
    public class RentCalculator : IRentCalculator
    {
        private readonly decimal _maxPrice;

        public RentCalculator(decimal maxPrice)
        {
            _maxPrice = maxPrice;
        }

        public decimal CalculateIncome(IList<Ride> rideHistory)
        {
            return rideHistory.Select(ride => ride.RidePrice).Sum();
        }

        public decimal CalculateRentalPrice(DateTime start, DateTime end, decimal pricePerMinute)
        {
            if (start >= end) throw new IncorrectTimeException("End time should be greater than start time!");

            var rentalDayCount = (end.Date - start.Date).TotalDays;

            return rentalDayCount > 1
                ? CalculateMultiDayRental(start, end, pricePerMinute)
                : CalculateSingleDayRental(start, end, pricePerMinute);
        }

        private decimal CalculateSingleDayRental(DateTime start, DateTime end, decimal pricePerMinute)
        {
            var rentalPrice = (decimal) (end - start).TotalMinutes * pricePerMinute;
            return rentalPrice > _maxPrice ? _maxPrice : rentalPrice;
        }

        private decimal CalculateMultiDayRental(DateTime start, DateTime end, decimal pricePerMinute)
        {
            return GetFirstDayPrice(start, pricePerMinute) + GetLastDayPrice(end, pricePerMinute) +
                   GetFullDaysPrice(start, end);
        }

        private decimal GetFullDaysPrice(DateTime start, DateTime end)
        {
            return (decimal) ((end.Date - start.Date).TotalDays - 1) * _maxPrice;
        }

        private decimal GetFirstDayPrice(DateTime start, decimal pricePerMinute)
        {
            const int totalMinutesInADay = 24 * 60;
            var firstDayPrice = (decimal) (totalMinutesInADay - start.TimeOfDay.TotalMinutes) * pricePerMinute;
            return firstDayPrice >= _maxPrice ? _maxPrice : 0M;
        }

        private decimal GetLastDayPrice(DateTime end, decimal pricePerMinute)
        {
            var secondDayPrice = (decimal) end.TimeOfDay.TotalMinutes * pricePerMinute;
            return secondDayPrice > _maxPrice ? _maxPrice : secondDayPrice;
        }
    }
}