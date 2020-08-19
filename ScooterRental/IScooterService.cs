using System.Collections.Generic;

namespace ScooterRental
{
    public interface IScooterService
    {
        void AddScooter(string id, decimal pricePerMinute);
        void RemoveScooter(string id);
        IList<Scooter> GetScooters();
        Scooter GetScooterById(string id);
    }
}