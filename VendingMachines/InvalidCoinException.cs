using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
