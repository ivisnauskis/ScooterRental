using System;

namespace ScooterRental.Exceptions
{
    public class IncorrectPriceException : Exception
    {
        public IncorrectPriceException(string? message) : base(message)
        {
        }
    }
}