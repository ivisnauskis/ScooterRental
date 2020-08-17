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
        public void AddScooter_SameId_ShouldThrowNotUniqueIdException()
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
    }
}

