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

        [Fact]
        public void AddScooter_UniqueId_Successful()
        {
            IScooterService service = new ScooterService();

            service.AddScooter("1", 0.2M);
            var scooterList = service.GetScooters();

            Assert.True(scooterList.Count == 1);
        }

        [Fact]
        public void AddScooter_SameId_ShouldFail()
        {
            IScooterService service = new ScooterService();

            service.AddScooter("1", 0.2M);
            service.AddScooter("1", 0.2M);
            var scooterList = service.GetScooters();

            Assert.True(scooterList.Count == 1);
        }

        [Fact]
        public void AddScooter_PriceBelowZero_ShouldFail()
        {
            IScooterService service = new ScooterService();
            
            service.AddScooter("1", -1M);
            var scooterList = service.GetScooters();

            Assert.Empty(scooterList);
        }





        //
        // [Fact]
        // public void RemoveScooter_CorrectID_ShouldBeSuccessful()
        // {
        //     IScooterService service = new ScooterService();
        //
        //     service.AddScooter("1", 0.2M);
        //     var scooterList = service.GetScooters();
        //
        //
        //     Assert.True(scooterList.Count == 1);
        // }
    }
}

