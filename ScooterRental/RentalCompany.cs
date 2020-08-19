using System;
using System.Collections.Generic;
using System.Linq;
using ScooterRental.Exceptions;

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

            _activeRides.Add(scooterToRent.Id, new Ride(scooterToRent, DateTime.Now));
            scooterToRent.IsRented = true;
        }

        public decimal EndRent(string id)
        {
            var rentedScooter = _service.GetScooterById(id);
            if (!rentedScooter.IsRented) throw new ScooterNotRentedException($"Scooter \"{id}\" is not rented.");

            var rideToEnd = _activeRides[id];
            var endTime = DateTime.Now;
            var ridePrice =
                _calculator.CalculateRentalPrice(rideToEnd.RideStartTime, endTime, rentedScooter.PricePerMinute);
            rideToEnd.EndRide(endTime, ridePrice);
            rentedScooter.IsRented = false;
            _activeRides.Remove(id);
            _rideHistory.Add(rideToEnd);

            return ridePrice;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {

            if (!includeNotCompletedRentals)
            {
                var rideHistory = year == null ? _rideHistory : _rideHistory.Where(ride => ride.RideEndTime.Year == year).ToList();
                return _calculator.CalculateIncome(rideHistory);
            }

            if (includeNotCompletedRentals)
            {
                var endTime = DateTime.Now;
                var rideHistory = year == null ? _rideHistory : _rideHistory.Where(ride => ride.RideEndTime.Year == year).ToList();

                var activeRidesPrice = year == endTime.Year || year == null?
                    _activeRides.Values.ToList()
                        .Select(i =>
                            _calculator.CalculateRentalPrice(i.RideStartTime, endTime, i.Scooter.PricePerMinute)).Sum() :
                    0;
                return _calculator.CalculateIncome(rideHistory) + activeRidesPrice;
            }

            return 0M;

        }
    }
}