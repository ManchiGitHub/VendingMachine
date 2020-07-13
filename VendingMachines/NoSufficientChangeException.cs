using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Exceptions
{
    public class NoSufficientChangeException : Exception
    {
        // Constructor
        public NoSufficientChangeException() : base(
            string.Format(
                @"not enough change in the machine. Client was refunded.{0}",
                Environment.NewLine))
        {
        }
    }
}
