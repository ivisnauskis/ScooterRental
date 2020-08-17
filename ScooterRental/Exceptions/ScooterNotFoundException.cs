using System;

namespace ScooterRental.tests
{
    public class ScooterNotFoundException : Exception
    {
        public ScooterNotFoundException(string? message) : base(message)
        {
        }
    }
}