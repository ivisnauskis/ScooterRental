using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using ScooterRental.Interfaces;
using Xunit;

namespace ScooterRental.tests
{
    public class RentalCompanyTests
    {
        private readonly Mock<IRentCalculator> _calculator;
        private readonly IRentalCompany _company;
        private readonly Mock<IRideService> _rideService;
        private readonly Mock<IScooterService> _scooterService;

        public RentalCompanyTests()
        {
            _scooterService = new Mock<IScooterService>();
            _calculator = new Mock<IRentCalculator>();
            _rideService = new Mock<IRideService>();

            ScooterServiceSetup();
            _company = new RentalCompany("title", _scooterService.Object, _calculator.Object, _rideService.Object);
        }

        [Fact]
        public void NameTest()
        {
            _company.Name.Should().Be("title");
        }

        [Fact]
        public void StartRent_ScooterNotRented_ShouldStartRent()
        {
            var scooter = _scooterService.Object.GetScooterById("1");
            _company.StartRent("1");
            _rideService.Verify(rs => rs.StartRide(scooter), Times.Once);
        }


        [Fact]
        public void EndRent_ScooterIsRented_ShouldEndRent()
        {
            _company.EndRent("1");
            _rideService.Verify(rs => rs.EndRide("1"), Times.Once);
        }

        [Fact]
        public void EndRent_GetPrice()
        {
            _rideService.Setup(rs => rs.EndRide("1")).Returns(2M);
            _company.EndRent("1").Should().Be(2M);
        }

        [Theory]
        [InlineData(2018, 35, 35, false)]
        [InlineData(2019, 4, 4, false)]
        [InlineData(2020, 13.5, 13.5, false)]
        [InlineData(2018, 35, 37, true)]
        [InlineData(2019, 4, 4, true)]
        [InlineData(2020, 13.5, 19.5, true)]
        [InlineData(null, 52.5, 60.5, true)]
        [InlineData(null, 52.5, 52.5, false)]
        public void CalculateIncome(int? year, decimal expectedNotIncludingActiveRides, decimal expectedTotal,
            bool include)
        {
            var rideHistory = year == null
                ? GetRideHistory()
                : GetRideHistory().Where(it => it.EndTime.Year == year).ToList();

            RideServiceSetup(rideHistory, year);
            CalculatorSetup(rideHistory, expectedNotIncludingActiveRides);

            _company.CalculateIncome(year, include).Should().Be(expectedTotal);
        }

        private void CalculatorSetup(List<Ride> rideHistory, decimal toReturn)
        {
            _calculator.Setup(x => x.CalculateIncome(rideHistory)).Returns(toReturn);
            _calculator.Setup(x =>
                x.CalculateRentalPrice(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0.2M)).Returns(2);
        }

        private void RideServiceSetup(List<Ride> rideHistory, int? year)
        {
            _rideService.Setup(rs => rs.GetRideHistory(year)).Returns(rideHistory);

            var count = year == null
                ? GetActiveRides().Count
                : GetActiveRides().Values.Count(ride => ride.StartTime.Year == year);
            var expected = count * 2M;

            _rideService.Setup(rs => rs.GetActiveRidesPrice(It.IsAny<int?>())).Returns(expected);
        }

        private void ScooterServiceSetup()
        {
            var scooter = new Scooter("1", 0.2M);
            _scooterService.Setup(x => x.GetScooterById("1")).Returns(scooter);
        }

        private Dictionary<string, Ride> GetActiveRides()
        {
            return new Dictionary<string, Ride>
            {
                {"7", new Ride(new Scooter("7", 0.2M), new DateTime(2020, 8, 1))},
                {"8", new Ride(new Scooter("8", 0.2M), new DateTime(2020, 8, 1))},
                {"9", new Ride(new Scooter("9", 0.2M), new DateTime(2020, 8, 1))},
                {"10", new Ride(new Scooter("10", 0.2M), new DateTime(2018, 8, 1))}
            };
        }

        private List<Ride> GetRideHistory()
        {
            var r1 = new Ride(new Scooter("1", 0.2M), new DateTime(2019, 1, 1));
            r1.EndRide(new DateTime(2019, 1, 1), 1.5M);
            var r2 = new Ride(new Scooter("2", 0.2M), new DateTime(2019, 1, 1));
            r2.EndRide(new DateTime(2019, 1, 1), 2.5M);
            var r3 = new Ride(new Scooter("3", 0.2M), new DateTime(2020, 1, 1));
            r3.EndRide(new DateTime(2020, 1, 1), 3.5M);
            var r4 = new Ride(new Scooter("4", 0.2M), new DateTime(2020, 1, 1));
            r4.EndRide(new DateTime(2020, 1, 1), 10M);
            var r5 = new Ride(new Scooter("5", 0.2M), new DateTime(2018, 1, 1));
            r5.EndRide(new DateTime(2018, 1, 1), 20M);
            var r6 = new Ride(new Scooter("6", 0.2M), new DateTime(2018, 1, 1));
            r6.EndRide(new DateTime(2018, 1, 1), 15M);
            return new List<Ride> {r1, r2, r3, r4, r5, r6};
        }
    }
}