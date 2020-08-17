using System;
using System.Collections.Generic;
using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        private readonly Dictionary<string, DateTime> _rentedScooterStartTimes;
        private readonly IScooterService _service;


        public RentalCompany(string name, IScooterService service)
        {
            Name = name;
            _service = service;
            _rentedScooterStartTimes = new Dictionary<string, DateTime>();
        }

        public string Name { get; }

        public void StartRent(string id)
        {
            var scooterToRent = _service.GetScooterById(id);
            if (scooterToRent.IsRented) throw new ScooterRentalInProgressException($"Scooter \"{id}\" already rented.");

            scooterToRent.IsRented = true;
        }

        public decimal EndRent(string id)
        {
            var rentedScooter = _service.GetScooterById(id);
            if (!rentedScooter.IsRented) throw new ScooterNotRentedException($"Scooter \"{id}\" already rented.");

            return decimal.Zero;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            throw new NotImplementedException();
        }
    }
}