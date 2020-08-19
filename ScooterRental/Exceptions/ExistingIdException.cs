using System;

namespace ScooterRental.Exceptions
{
    public class ExistingIdException : Exception
    {
        public ExistingIdException(string? message) : base(message)
        {
        }
    }
}