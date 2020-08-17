using System;
using System.Collections.Generic;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        private IScooterService _service;
        
        public RentalCompany(string name, IScooterService service)
        {
            Name = name;
            _service = service;
        }

        public string Name { get; }
        public void StartRent(string id)
        {
            _service.GetScooterById(id).IsRented = true;

        }

        public decimal EndRent(string id)
        {
            throw new System.NotImplementedException();
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            throw new System.NotImplementedException();
        }
    }
}