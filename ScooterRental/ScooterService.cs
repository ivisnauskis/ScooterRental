using System.Collections.Generic;
using ScooterRental.Exceptions;
using ScooterRental.tests;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private const decimal MinimalPrice = 0.01M;
        private readonly List<Scooter> _scooters;

        public ScooterService()
        {
            _scooters = new List<Scooter>();
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (_scooters.Find(scooter => scooter.Id == id) != null)
                throw new ExistingIdException("Scooter with this ID already saved.");

            if (pricePerMinute < MinimalPrice)
                throw new IncorrectPriceException($"Scooter price should be above {MinimalPrice}");

            _scooters.Add(new Scooter(id, pricePerMinute));
        }

        public void RemoveScooter(string id)
        {
            var scooterToRemove = _scooters.Find(scooter => scooter.Id == id);
            if (scooterToRemove == null) throw new ScooterNotFoundException($"Scooter by ID: {id} not found!");
            if (scooterToRemove.IsRented) throw new ScooterRentalInProgressException("Scooter rental in progress. Cannot be deleted.");
            _scooters.Remove(scooterToRemove);
        }

        public IList<Scooter> GetScooters()
        {
            return _scooters;
        }

        public Scooter GetScooterById(string id)
        {
            var scooterToReturn = _scooters.Find(scooter => scooter.Id == id);
            if (scooterToReturn == null) throw new ScooterNotFoundException($"Scooter by ID: {id} not found!");

            return scooterToReturn;
        }
    }
}