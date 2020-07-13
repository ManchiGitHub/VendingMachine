namespace VendingMachine
{
    public class Bucket<Money, Item>
    {
        private Money m_Change;
        private Item m_Product;

        // Constructor
        public Bucket(Money i_Change, Item i_Product)
        {
            m_Change = i_Change;
            m_Product = i_Product;
        }

        // Properties
        public Money Change
        {
            get { return m_Change; }

            set { m_Change = value; }
        }

        public Item Product
        {
            get { return m_Product; }

            set { m_Product = value; }
        }
    }
}