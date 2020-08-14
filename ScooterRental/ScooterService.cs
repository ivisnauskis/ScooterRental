using System.Collections.Generic;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private List<Scooter> _scooters;

        public ScooterService()
        {
            _scooters = new List<Scooter>();
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveScooter(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<Scooter> GetScooters()
        {
            throw new System.NotImplementedException();
        }

        public Scooter GetScooterById(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}