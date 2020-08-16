using System.Collections.Generic;
using System.Linq;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private readonly List<Scooter> _scooters;

        public ScooterService()
        {
            _scooters = new List<Scooter>();
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (_scooters.Find(it => it.Id == id) == null && pricePerMinute > 0)
            {
                _scooters.Add(new Scooter(id, pricePerMinute));
            }
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