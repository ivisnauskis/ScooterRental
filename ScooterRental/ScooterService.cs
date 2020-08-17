using System.Collections.Generic;
using System.Linq;
using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private readonly List<Scooter> _scooters;
        private const decimal MinimalPrice = 0.01M;

        public ScooterService()
        {
            _scooters = new List<Scooter>();
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (_scooters.Find(scooter => scooter.Id == id) != null)
            {
                throw new ExistingIdException("Scooter with this ID already saved.");
            }

            if (pricePerMinute < MinimalPrice)
            {
                throw new IncorrectPriceException($"Scooter price should be above {MinimalPrice}");
            }

            _scooters.Add(new Scooter(id, pricePerMinute));
        }

        public void RemoveScooter(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Scooter> GetScooters()
        {
            return _scooters;
        }

        public Scooter GetScooterById(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}