using System;
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
    }
}
