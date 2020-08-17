using System;
using System.Diagnostics;
using ScooterRental.Exceptions;
using Xunit;

namespace ScooterRental.tests
{
    public class RentalCompanyTests
    {
        [Fact]
        public void StartRent()
        {
            IScooterService service = new ScooterService();
            IRentalCompany company = new RentalCompany("rental", service);
            service.AddScooter("1", 0.2M);
            company.StartRent("1");
            var result = service.GetScooterById("1").IsRented;
            
            Assert.True(result);
        }

    }
}

