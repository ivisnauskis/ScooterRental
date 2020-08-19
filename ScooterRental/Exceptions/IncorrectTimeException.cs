using System;

namespace ScooterRental.Exceptions
{
    public class IncorrectTimeException : Exception
    {
        public IncorrectTimeException(string? message) : base(message)
        {
        }
    }
}