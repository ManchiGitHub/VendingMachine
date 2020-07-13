using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Exceptions
{
    public class SoldOutException : Exception
    {
        // Constructor
        public SoldOutException() : base(
            string.Format(
                @"Product is sold out. Please choose a different product.{0}",
                Environment.NewLine))
        {
        }
    }
}
