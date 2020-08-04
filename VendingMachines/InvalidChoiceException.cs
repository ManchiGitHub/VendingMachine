using System;

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
