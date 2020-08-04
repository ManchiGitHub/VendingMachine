using System;

namespace VendingMachine.Exceptions
{
    public class InvalidCoinException : Exception
    {
        // Constructor
        public InvalidCoinException() : base(
            string.Format(
                @"Coin is invalid.{0}",
                Environment.NewLine))
        {
        }
    }
}
