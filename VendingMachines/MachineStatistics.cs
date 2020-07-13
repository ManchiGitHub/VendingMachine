namespace VendingMachine
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MachineStatistics
    {
        private int m_TotalRevenue = 0;
        private int m_CashInMachine = 0;
        private List<eProduct> m_ProductsSold = new List<eProduct>();

        // Constructor 
        public MachineStatistics()
        {
        }

        // Properties
        public int TotalRevenue
        {
            get { return m_TotalRevenue; }

            set { m_TotalRevenue = value; }
        }

        public int CashInMachine
        {
            get { return m_CashInMachine; }

            set { m_CashInMachine = value; }
        }

        public List<eProduct> ProductsSold
        {
            get { return m_ProductsSold; }

            set { m_ProductsSold = value; }
        }

        private string ProductsSoldString(List<eProduct> i_ListOfProducts)
        {
            StringBuilder productsString = new StringBuilder();

            foreach (eProduct product in Enum.GetValues(typeof(eProduct)))
            {
                productsString.Append(product);
                productsString.Append(Environment.NewLine);
            }

            return productsString.ToString();
        }

        public string StringifyStatistics()
        {
            string statistics = string.Empty;

            string.Format(
                @"Total Revenue: {1} Shekels.{0}
                Current amount of money in machine: {2} Shekels.{0}
                Products Sold: {3}",
                Environment.NewLine,
                TotalRevenue,
                CashInMachine,
                ProductsSoldString(ProductsSold));

            return statistics;
        }
    }
}
