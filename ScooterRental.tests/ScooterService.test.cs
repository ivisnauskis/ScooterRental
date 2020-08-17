using System.Linq;
using ScooterRental.Exceptions;
using Xunit;

namespace ScooterRental.tests
{
    public class ScooterServiceTests
    {
        [Fact]
        public void GetScooters_ReturnsEmptyList()
        {
            IScooterService service = new ScooterService();
            var scooterList = service.GetScooters();
            Assert.Empty(scooterList);
        }

        [Fact]
        public void AddScooter_UniqueId_Successful()
        {
            IScooterService service = new ScooterService();

            service.AddScooter("1", 0.2M);
            var scooterList = service.GetScooters();

            Assert.Equal(1, scooterList.Count);
        }

        [Fact]
        public void AddScooter_SameId_ShouldThrowExistingIdException()
        {
            IScooterService service = new ScooterService();

            service.AddScooter("1", 0.2M);

            Assert.Throws<ExistingIdException>(() => service.AddScooter("1", 0.2M));
        }

        [Fact]
        public void AddScooter_NegativePrice_ShouldThrowIncorrectPriceException()
        {
            IScooterService service = new ScooterService();
            
            Assert.Throws<IncorrectPriceException>(() => service.AddScooter("1", -1M));
        }

        [Fact]
        public void RemoveScooter_CorrectId_ShouldRemove()
        {
            IScooterService service = new ScooterService();
            service.AddScooter("1", 0.2M);

            Assert.Equal(1, service.GetScooters().Count);
            service.RemoveScooter("1");
            Assert.Empty(service.GetScooters());
        }

        [Fact]
        public void RemoveScooter_IncorrectId_ShouldThrowScooterNotFoundException()
        {
            IScooterService service = new ScooterService();

            Assert.Throws<ScooterNotFoundException>(() => service.RemoveScooter("1"));
        }

        [Fact]
        public void RemoveScooter_RentalInProgress_ShouldThrowScooterRentalInProgressException()
        {
            IScooterService service = new ScooterService();
            service.AddScooter("1", 0.2M);
            service.GetScooters().First(it => it.Id == "1").IsRented = true;

            Assert.Throws<ScooterRentalInProgressException>(() => service.RemoveScooter("1"));
        }

        [Fact]
        public void GetScooterById_CorrectId_ShouldReturnScooter()
        {
            IScooterService service = new ScooterService();
            service.AddScooter("1", 0.2M);

            var scooter = service.GetScooterById("1");
            Assert.Equal("1", scooter.Id);
        }

        [Fact]
        public void GetScooterById_IncorrectId_ShouldThrowScooterNotFoundException()
        {
            IScooterService service = new ScooterService();

            Assert.Throws<ScooterNotFoundException>(() => service.GetScooterById("1"));
        }
    }
}

