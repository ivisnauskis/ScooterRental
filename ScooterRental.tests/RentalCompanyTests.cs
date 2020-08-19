using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ScooterRental.Exceptions;
using Xunit;

namespace ScooterRental.tests
{
    public class RentalCompanyTests
    {
        private IRentalCompany _company;
        private readonly Mock<IScooterService> _service;
        private readonly Mock<IRentCalculator> _calculator;

        public RentalCompanyTests()
        {
            _service = new Mock<IScooterService>();
            _calculator = new Mock<IRentCalculator>();
            _company = new RentalCompany("title", _service.Object, _calculator.Object);
        }

        [Fact]
        public void NameTest()
        {
            var company = new RentalCompany("Scooter", _service.Object, _calculator.Object);
            Assert.Equal("Scooter", company.Name);
        }

        [Fact]
        public void StartRent_ScooterNotRented_ShouldStartRent()
        {
            var scooter = new Scooter("1", 0.2M);
            _service.Setup(x => x.GetScooterById("1")).Returns(scooter);
            
            Assert.False(scooter.IsRented);
            _company.StartRent("1");

            Assert.True(scooter.IsRented);
        }

        [Fact]
        public void StartRent_ScooterAlreadyRented_ShouldThrowScooterRentalInProgressException()
        {
            var scooter = new Scooter("1", 0.2M) {IsRented = true};
            _service.Setup(x => x.GetScooterById("1")).Returns(scooter);

            Assert.Throws<ScooterRentalInProgressException>(() => _company.StartRent("1"));
        }

        [Fact]
        public void EndRent_ScooterIsRented_ShouldEndRent()
        {
            var scooter = new Scooter("1", 0.2M);
            _service.Setup(x => x.GetScooterById("1")).Returns(scooter);
            _company.StartRent("1");
            _company.EndRent("1");
            Assert.False(scooter.IsRented);
        }
        
        [Fact]
        public void EndRent_ScooterNotRented_ShouldThrowScooterNotRentedException()
        {
            var scooter = new Scooter("1", 0.2M);
            _service.Setup(x => x.GetScooterById("1")).Returns(scooter);

            Assert.False(scooter.IsRented);
            Assert.Throws<ScooterNotRentedException>(() => _company.EndRent("1"));
        }
        
        [Fact]
        public void EndRent_GetPrice()
        {
            var scooter = new Scooter("1", 0.2M);
            _service.Setup(x => x.GetScooterById("1")).Returns(scooter);
            _calculator
                .Setup(x => x.CalculateRentalPrice(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0.2M))
                .Returns(15M);

            _company.StartRent("1");

            var rentPrice = _company.EndRent("1");
        
            Assert.Equal(15M, rentPrice);
        }
        
        [Fact]
        public void CalculateIncome_AllYearsActiveRentalsNotIncluded()
        {
            var rideHistory = GetRideHistory();
            _calculator.Setup(x => x.CalculateIncome(rideHistory)).Returns(52.5M);
            _company = new RentalCompany(GetActiveRides(), _calculator.Object, rideHistory, _service.Object, "rental");

            var income = _company.CalculateIncome(null, false);

            Assert.Equal(52.5M, income);
        }

        [Theory]
        [InlineData(2018, 35)]
        [InlineData(2019, 4)]
        [InlineData(2020, 13.5)]
        public void CalculateIncome_SpecificYearActiveRentalsNotIncluded(int year, decimal expected)
        {
            var rideHistory = GetRideHistory().Where(it => it.EndTime.Year == year).ToList();
            _calculator.Setup(x => x.CalculateIncome(rideHistory)).Returns(expected);
        
            _company = new RentalCompany(GetActiveRides(), _calculator.Object, rideHistory, _service.Object, "rental");
        
            var income = _company.CalculateIncome(year, false);
        
            Assert.Equal(expected, income);
        }

        [Theory]
        [InlineData(2018, 35, 35)]
        [InlineData(2019, 4, 4)]
        [InlineData(2020, 13.5, 19.5)]
        public void CalculateIncome_SpecificYearActiveIncluded(int year, decimal expected, decimal expectedTotal)
        {
            var rideHistory = GetRideHistory().Where(it => it.EndTime.Year == year).ToList();

            _calculator.Setup(x => x.CalculateIncome(rideHistory)).Returns(expected);
            _calculator.Setup(x => 
                x.CalculateRentalPrice(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0.2M)).Returns(2);

            _company = new RentalCompany(GetActiveRides(), _calculator.Object, rideHistory, _service.Object, "rental");

            Assert.Equal(expectedTotal, _company.CalculateIncome(year, true));
        }

        [Fact] public void CalculateIncome_AllYearsActiveIncluded()
        {
            var rideHistory = GetRideHistory();

            _calculator.Setup(x => x.CalculateIncome(rideHistory)).Returns(52.5M);
            _calculator.Setup(x =>
                x.CalculateRentalPrice(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0.2M)).Returns(2);

            _company = new RentalCompany(GetActiveRides(), _calculator.Object, rideHistory, _service.Object, "rental");

            Assert.Equal(58.5M, _company.CalculateIncome(null, true));
        }

        private Dictionary<string, Ride> GetActiveRides()
        {
            return new Dictionary<string, Ride>
            {
                {"7", new Ride(new Scooter("7", 0.2M), new DateTime(2020, 8, 1))},
                {"8", new Ride(new Scooter("8", 0.2M), new DateTime(2020, 8, 1))},
                {"9", new Ride(new Scooter("9", 0.2M), new DateTime(2020, 8, 1))}
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

            return new List<Ride>() {r1, r2, r3, r4, r5, r6};
        }
    }
}