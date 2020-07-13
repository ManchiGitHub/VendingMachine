using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Exceptions
{
    public class InvalidChoiceException : Exception
    {
        // Constructor
        public InvalidChoiceException() : base(
            string.Format(
                @"Invalid choice.{0}",
                Environment.NewLine))
        {
        }
    }
}
