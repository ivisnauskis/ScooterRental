using System.Collections.Generic;
using System.Linq;
using ScooterRental.Exceptions;
using ScooterRental.tests;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private const decimal MinimalPrice = 0.01M;
        private readonly Dictionary<string, Scooter> _scooters;

        public ScooterService()
        {
            _scooters = new Dictionary<string, Scooter>();
        }

        public ScooterService(Dictionary<string, Scooter> scooterInventory)
        {
            _scooters = scooterInventory;
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (_scooters.ContainsKey(id))
                throw new ExistingIdException("Scooter with this ID already saved.");

            if (pricePerMinute < MinimalPrice)
                throw new IncorrectPriceException($"Scooter price should be above {MinimalPrice}");

            _scooters.Add(id, new Scooter(id, pricePerMinute));
        }

        public void RemoveScooter(string id)
        {
            if (!_scooters.ContainsKey(id)) throw new ScooterNotFoundException($"Scooter by ID: {id} not found!");
            if (_scooters[id].IsRented)
                throw new ScooterRentalInProgressException("Scooter rental in progress. Cannot be deleted.");
            _scooters.Remove(id);
        }

        public IList<Scooter> GetScooters()
        {
            return _scooters.Values.ToList();
        }

        public Scooter GetScooterById(string id)
        {
            if (!_scooters.ContainsKey(id)) throw new ScooterNotFoundException($"Scooter by ID: {id} not found!");

            return _scooters[id];
        }
    }
}