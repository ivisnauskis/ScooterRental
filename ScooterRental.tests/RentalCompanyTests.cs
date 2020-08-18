using System;
using ScooterRental.Exceptions;
using Xunit;


namespace ScooterRental.tests
{
    public class RentalCompanyTests
    {
        private IScooterService service;
        private IRentalCompany company;
        private IRentCalculator accountant;

        public RentalCompanyTests()
        {
            service = new ScooterService();
            accountant = new RentCalculator(20M);
            company = new RentalCompany("title", service, accountant);
        }

        [Fact]
        public void StartRent_ScooterNotRented_ShouldStartRent()
        {
            service.AddScooter("1", 0.2M);
            company.StartRent("1");
            var result = service.GetScooterById("1").IsRented;
            
            Assert.True(result);
        }

        [Fact]
        public void StartRent_ScooterAlreadyRented_ShouldThrowScooterRentalInProgressException()
        {
            service.AddScooter("1", 0.2M);
            company.StartRent("1");

            Assert.Throws<ScooterRentalInProgressException>(() => company.StartRent("1"));
        }

        [Fact]
        public void EndRent_ScooterIsRented_ShouldEndRent()
        {
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
            service.AddScooter("1", 0.2M);
            var scooter = service.GetScooterById("1");

            Assert.False(scooter.IsRented);
            Assert.Throws<ScooterNotRentedException>(() => company.EndRent("1"));
        }

        [Fact]
        public void EndRend_GetPrice()
        {
            service.AddScooter("1", 0.2M);
            var scooter = service.GetScooterById("1");
            company.StartRent("1");
            
            Assert.Equal(0M, Math.Round(company.EndRent("1")));
        }

        [Fact]
        public void CalculateIncome()
        {

        }
    }
}