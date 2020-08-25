using System;
using System.Collections.Generic;
using System.Linq;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental
{
    public class RideService : IRideService
    {
        private readonly IRentCalculator _calculator;
        private readonly Dictionary<string, Ride> _activeRides = new Dictionary<string, Ride>();
        private readonly List<Ride> _rideHistory = new List<Ride>();

        public RideService(IRentCalculator calculator)
        {
            _calculator = calculator;
        }

        public RideService(IRentCalculator calculator, Dictionary<string, Ride> activeRides, List<Ride> rideHistory)
        {
            _calculator = calculator;
            _activeRides = activeRides;
            _rideHistory = rideHistory;
        }

        public IList<Ride> GetRideHistory(int? year)
        {
            return year.HasValue
                ? _rideHistory.Where(ride => ride.EndTime.Year == year).ToList()
                : _rideHistory;
        }

        public void StartRide(Scooter scooter)
        {
            if (scooter.IsRented || _activeRides.ContainsKey(scooter.Id)) 
                throw new ScooterRentalInProgressException($"Scooter \"{scooter.Id}\" already rented.");

            _activeRides.Add(scooter.Id, new Ride(scooter, DateTime.Now));
            scooter.IsRented = true;
        }

        public decimal EndRide(string scooterId)
        { 
            if (!_activeRides.ContainsKey(scooterId))
                throw new ScooterNotRentedException($"Scooter \"{scooterId}\" is not rented.");

            var ride = _activeRides[scooterId];
            var endTime = DateTime.Now;
            var price = _calculator.CalculateRentalPrice(ride.StartTime, endTime, ride.Scooter.PricePerMinute);
            ride.EndRide(endTime, price);
            ride.Scooter.IsRented = false;
            _activeRides.Remove(ride.Scooter.Id);
            _rideHistory.Add(ride);
            return price;
        }

        public decimal GetActiveRidesPrice(int? year)
        {
            var endTime = DateTime.Now;
            var rides = year.HasValue
                ? _activeRides.Values.Where(ride => ride.StartTime.Year == year)
                : _activeRides.Values;

            return rides
                .Sum(ride => _calculator.CalculateRentalPrice(ride.StartTime, endTime, ride.Scooter.PricePerMinute));
        }
    }
}