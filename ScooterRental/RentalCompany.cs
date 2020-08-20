using System;
using System.Collections.Generic;
using System.Linq;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        private readonly Dictionary<string, Ride> _activeRides = new Dictionary<string, Ride>();
        private readonly IRentCalculator _calculator;
        private readonly List<Ride> _rideHistory = new List<Ride>();
        private readonly IScooterService _service;

        public RentalCompany(string name, IScooterService service, IRentCalculator calculator)
        {
            _calculator = calculator;
            _service = service;
            Name = name;
        }

        public RentalCompany(Dictionary<string, Ride> activeRides, IRentCalculator calculator, List<Ride> rideHistory,
            IScooterService service, string name)
            : this(name, service, calculator)
        {
            _activeRides = activeRides;
            _rideHistory = rideHistory;
        }

        public string Name { get; }

        public void StartRent(string id)
        {
            var scooterToRent = _service.GetScooterById(id);
            if (scooterToRent.IsRented) throw new ScooterRentalInProgressException($"Scooter \"{id}\" already rented.");

            StartRide(scooterToRent);
        }

        public decimal EndRent(string id)
        {
            var rentedScooter = _service.GetScooterById(id);
            if (!rentedScooter.IsRented || !_activeRides.ContainsKey(id))
                throw new ScooterNotRentedException($"Scooter \"{id}\" is not rented.");

            return EndRide(_activeRides[id]);
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            var rideHistory =
                year == null ? _rideHistory : _rideHistory.Where(ride => ride.EndTime.Year == year).ToList();

            var completedRidesIncome = _calculator.CalculateIncome(rideHistory);

            return completedRidesIncome + (includeNotCompletedRentals ? GetActiveRidesPrice(year) : 0);
        }

        private decimal GetActiveRidesPrice(int? year)
        {
            var endTime = DateTime.Now;
            var rides = year == null
                ? _activeRides.Values
                : _activeRides.Values.Where(ride => ride.StartTime.Year == year);

            return rides
                .Select(ride => _calculator.CalculateRentalPrice(ride.StartTime, endTime, ride.Scooter.PricePerMinute))
                .Sum();
        }

        private void StartRide(Scooter scooter)
        {
            _activeRides.Add(scooter.Id, new Ride(scooter, DateTime.Now));
            scooter.IsRented = true;
        }

        private decimal EndRide(Ride ride)
        {
            var endTime = DateTime.Now;
            var price =
                _calculator.CalculateRentalPrice(ride.StartTime, endTime, ride.Scooter.PricePerMinute);
            ride.EndRide(endTime, price);
            ride.Scooter.IsRented = false;
            _activeRides.Remove(ride.Scooter.Id);
            _rideHistory.Add(ride);
            return price;
        }
    }
}