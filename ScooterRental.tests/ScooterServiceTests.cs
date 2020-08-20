using System;
using System.Linq;
using FluentAssertions;
using ScooterRental.Exceptions;
using Xunit;

namespace ScooterRental.tests
{
    public class ScooterServiceTests
    {
        private IScooterService service;

        public ScooterServiceTests()
        {
            service = new ScooterService();
        }

        [Fact]
        public void GetScooters_ReturnsEmptyList()
        {
            var scooterList = service.GetScooters();
            scooterList.Should().BeEmpty();
        }

        [Fact]
        public void AddScooter_UniqueId_Successful()
        {
            service.AddScooter("1", 0.2M);
            var scooterList = service.GetScooters();
            scooterList.Count.Should().Be(1);
        }

        [Fact]
        public void AddScooter_SameId_ShouldThrowExistingIdException()
        {
            service.AddScooter("1", 0.2M);
            Action act = () => service.AddScooter("1", 0.2M);
            act.Should().Throw<ExistingIdException>();
        }

        [Fact]
        public void AddScooter_NegativePrice_ShouldThrowIncorrectPriceException()
        {
            Action act = () => service.AddScooter("1", -1M);
            act.Should().Throw<IncorrectPriceException>();
        }

        [Fact]
        public void RemoveScooter_CorrectId_ShouldRemove()
        {
            service.AddScooter("1", 0.2M);

            service.GetScooters().Count.Should().Be(1);
            service.RemoveScooter("1");
            service.GetScooters().Should().BeEmpty();
        }

        [Fact]
        public void RemoveScooter_IncorrectId_ShouldThrowScooterNotFoundException()
        {
            Action act = () => service.RemoveScooter("1");
            act.Should().Throw<ScooterNotFoundException>();
        }

        [Fact]
        public void RemoveScooter_RentalInProgress_ShouldThrowScooterRentalInProgressException()
        {
            service.AddScooter("1", 0.2M);
            service.GetScooters().First(it => it.Id == "1").IsRented = true;

            Action act = () => service.RemoveScooter("1");
            act.Should().Throw<ScooterRentalInProgressException>();
        }

        [Fact]
        public void GetScooterById_CorrectId_ShouldReturnScooter()
        {
            service.AddScooter("1", 0.2M);

            var scooter = service.GetScooterById("1");
            scooter.Id.Should().Be("1");
        }

        [Fact]
        public void GetScooterById_IncorrectId_ShouldThrowScooterNotFoundException()
        {
            Action act = () => service.GetScooterById("1");
            act.Should().Throw<ScooterNotFoundException>();
        }
    }
}
