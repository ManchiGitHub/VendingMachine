namespace VendingMachine
{
    using System;
    using System.Collections.Generic;
    using VendingMachine.Exceptions;

    public class NewVendingMachine : IVendingMachines
    {
        private Inventory<eCoin> m_CoinInventory = new Inventory<eCoin>();
        private Inventory<eProduct> m_ProductInventory = new Inventory<eProduct>();
        private eProduct? m_CurrentProduct;
        private int m_TotalMachineRevenue = 0;
        private int m_CurrentBalance = 0;

        // Constructor
        public NewVendingMachine()
        {
        }

        public eProduct CurrentProduct
        {
            get { return (eProduct)m_CurrentProduct; }

            set { m_CurrentProduct = value; }
        }

        public int TotalMachineRevenue
        {
            get { return m_TotalMachineRevenue; }

            set { m_TotalMachineRevenue = value; }
        }

        public int CurrentBalance
        {
            get { return m_CurrentBalance; }

            set { m_CurrentBalance = value; }
        }

        public Inventory<eCoin> CoinInventory
        {
            get { return m_CoinInventory; }

            set { m_CoinInventory = value; }
        }

        public Inventory<eProduct> ProductInventory
        {
            get { return m_ProductInventory; }

            set { m_ProductInventory = value; }
        }

        public void RefillAll(int i_counter)
        {
            for (int i = 0; i < i_counter; i++)
            {
                foreach (eCoin coin in Enum.GetValues(typeof(eCoin)))
                {
                    CoinInventory.AddItem(coin);
                }

                foreach (eProduct product in Enum.GetValues(typeof(eProduct)))
                {
                    m_ProductInventory.AddItem(product);
                }
            }
        }

        public void InsertCoin(eCoin i_Coin)
        {
            m_CurrentBalance += (int)i_Coin;
            CoinInventory.AddItem(i_Coin);
        }

        public void userChoiceSwitchCase(eConfirmOrRefund i_userChoice)
        {
            switch (i_userChoice)
            {
                case eConfirmOrRefund.Confirm:
                    CollectChangeAndItem();
                    break;

                case eConfirmOrRefund.Refund:
                    Refund();
                    break;
                default:
                    break;
            }
        }

        public bool isSufficientFunds()
        {
            return m_CurrentBalance >= (int)m_CurrentProduct;
        }

        public List<eCoin> Refund()
        {
            List<eCoin> refundList = GetChange(m_CurrentBalance);
            m_CurrentBalance = 0;
            m_CurrentProduct = null;

            return refundList;
        }

        public List<eCoin> GetChange(int i_Amount)
        {
            List<eCoin> changeList = new List<eCoin>();
            int balance = i_Amount;

            if (i_Amount > 0)
            {
                while (balance > 0)
                {
                    if (balance >= (int)eCoin.QUARTER && CoinInventory.getQuantity(eCoin.QUARTER) > 0)
                    {
                        changeAndCoinInventoryHandler(eCoin.QUARTER, ref changeList, ref balance);
                        continue;
                    }
                    else if (balance >= (int)eCoin.DIME && CoinInventory.getQuantity(eCoin.DIME) > 0)
                    {
                        changeAndCoinInventoryHandler(eCoin.DIME, ref changeList, ref balance);
                        continue;
                    }
                    else if (balance >= (int)eCoin.NICKEL && CoinInventory.getQuantity(eCoin.NICKEL) > 0)
                    {
                        changeAndCoinInventoryHandler(eCoin.NICKEL, ref changeList, ref balance);
                        continue;
                    }
                    else if (balance >= (int)eCoin.PENNY && CoinInventory.getQuantity(eCoin.PENNY) > 0)
                    {
                        changeAndCoinInventoryHandler(eCoin.PENNY, ref changeList, ref balance);
                        continue;
                    }
                    else
                    {
                        Refund();
                        throw new NoSufficientChangeException();
                    }
                }
            }

            return changeList;
        }

        private void changeAndCoinInventoryHandler(eCoin i_eCoin, ref List<eCoin> io_ChangeList, ref int io_balance)
        {
            io_ChangeList.Add(i_eCoin);
            io_balance -= (int)i_eCoin;
            CoinInventory.RemoveItem(i_eCoin);
        }

        public Bucket<List<eCoin>, eProduct> CollectChangeAndItem()
        {
            List<eCoin> change = GetChange(m_CurrentBalance - (int)m_CurrentProduct);
            m_ProductInventory.RemoveItem((eProduct)m_CurrentProduct);
            m_TotalMachineRevenue += (int)m_CurrentProduct;
            m_CurrentBalance = 0;

            return new Bucket<List<eCoin>, eProduct>(change, (eProduct)m_CurrentProduct);
        }

        public void ResetMachine()
        {
            CoinInventory.ClearInventory();
            m_ProductInventory.ClearInventory();
            m_TotalMachineRevenue = 0;
            m_CurrentProduct = null;
            m_CurrentBalance = 0;
        }
    }
}
