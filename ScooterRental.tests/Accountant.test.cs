using System;
using System.Diagnostics;
using ScooterRental.Exceptions;
using Xunit;

namespace ScooterRental.tests
{
    public class RentalCompanyTests
    {
        [Fact]
        public void StartRent_ScooterNotRented_ShouldStartRent()
        {
            IScooterService service = new ScooterService();
            IRentalCompany company = new RentalCompany("rental", service);
            service.AddScooter("1", 0.2M);
            company.StartRent("1");
            var result = service.GetScooterById("1").IsRented;
            
            Assert.True(result);
        }

        [Fact]
        public void StartRent_ScooterAlreadyRented_ShouldThrowScooterRentalInProgressException()
        {
            IScooterService service = new ScooterService();
            IRentalCompany company = new RentalCompany("rental", service);
            service.AddScooter("1", 0.2M);
            company.StartRent("1");

            Assert.Throws<ScooterRentalInProgressException>(() => company.StartRent("1"));
        }

        [Fact]
        public void EndRent_ScooterIsRented_ShouldEndRent()
        {
            IScooterService service = new ScooterService();
            IRentalCompany company = new RentalCompany("rental", service);
            service.AddScooter("1", 0.2M);
            var scooter = service.GetScooterById("1");

            company.StartRent("1");
            Assert.True(scooter.IsRented);

            company.EndRent("1");
            Assert.False(scooter.IsRented);
        }

        [Fact]
        public void EndRent_ScooterNotRented_ShouldThrowScooterNotRentedException()
        {
            IScooterService service = new ScooterService();
            IRentalCompany company = new RentalCompany("rental", service);
            service.AddScooter("1", 0.2M);
            var scooter = service.GetScooterById("1");

            Assert.False(scooter.IsRented);
            Assert.Throws<ScooterNotRentedException>(() => company.EndRent("1"));
        }

        [Fact]
        public void EndRent_ScooterRented_ShouldReturnRentSum()
        {
            IScooterService service = new ScooterService();
            IRentalCompany company = new RentalCompany("rental", service);
            service.AddScooter("1", 0.2M);

            company.StartRent("1");
            var startTime = DateTime.Now;

            var endTime = startTime.AddMinutes(5);

            var rentalCompany = (RentalCompany) company; 
            var res = rentalCompany.CalculateRentSum(startTime, endTime, 0.2M);

            Assert.Equal(0, company.EndRent("1"));
            Assert.Equal(1, res);
        }
    }
}

