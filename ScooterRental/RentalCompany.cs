using System.Linq;
using ScooterRental.Interfaces;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        private readonly IRentCalculator _calculator;
        private readonly IRideService _rideService;
        private readonly IScooterService _scooterService;

        public RentalCompany(string name, IScooterService scooterService, IRentCalculator calculator,
            IRideService rideService)
        {
            Name = name;
            _calculator = calculator;
            _scooterService = scooterService;
            _rideService = rideService;
        }

        public string Name { get; }

        public void StartRent(string id)
        {
            var scooterToRent = _scooterService.GetScooterById(id);
            _rideService.StartRide(scooterToRent);
        }

        public decimal EndRent(string id)
        {
            var price = _rideService.EndRide(id);
            return price;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            var rideHistory = _rideService.GetRideHistory(year);

            var completedRidesIncome = _calculator.CalculateIncome(rideHistory);

            return completedRidesIncome + (includeNotCompletedRentals ? _rideService.GetActiveRidesPrice(year) : 0);
        }
    }
}