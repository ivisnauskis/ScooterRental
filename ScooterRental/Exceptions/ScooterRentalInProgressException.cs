using System;

namespace ScooterRental.Exceptions
{
    public class ScooterRentalInProgressException : Exception
    {
        public ScooterRentalInProgressException(string? message) : base(message)
        {
        }
    }
}