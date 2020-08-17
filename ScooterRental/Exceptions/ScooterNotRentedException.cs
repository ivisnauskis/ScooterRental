using System;

namespace ScooterRental.Exceptions
{
    public class ScooterNotRentedException : Exception
    {
        public ScooterNotRentedException(string? message) : base(message)
        {
        }
    }
}