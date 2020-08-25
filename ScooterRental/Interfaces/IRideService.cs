using System.Collections.Generic;

namespace ScooterRental.Interfaces
{
    public interface IRideService
    {
        IList<Ride> GetRideHistory(int? year);

        void StartRide(Scooter scooter);

        decimal EndRide(string scooterId);

        decimal GetActiveRidesPrice(int? year);
    }
}