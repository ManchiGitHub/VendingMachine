namespace VendingMachineController
{
    using System;
    using CMDInterface;
    using VendingMachine;
    using VendingMachine.Exceptions;

    public class Controller
    {
        public static void Run()
        {
            NewVendingMachine vendingMachine = VendingMachineFactory.CreateNewVendingMachine();
            initializeMachine(vendingMachine);
        }

        private static void initializeMachine(NewVendingMachine i_VendingMachine)
        {
            int price;

            i_VendingMachine.RefillAll(10);

            while (true)
            {
                // Pre Selected...                  
                price = selectProductAndGetPrice(i_VendingMachine, eProduct.COKE);
                enterAmountMessage(price);

                recieveMoney(i_VendingMachine);
                confirmPurchaseOrRefund(i_VendingMachine);
            }
        }

        private static void recieveMoney(NewVendingMachine i_VendingMachine)
        {
            int? clientCoin = null;
            bool? isCoinValid = null;

            do
            {
                try
                {
                    clientCoin = int.Parse(getCoinFromUser());
                    isCoinValid = Enum.IsDefined(typeof(eCoin), clientCoin);

                    if (!(bool)isCoinValid)
                    {
                        throw new InvalidChoiceException();
                    }
                }
                catch (Exception ex)
                {
                    isCoinValid = false;

                    if (ex is FormatException)
                    {
                        invalidMessage();
                    }

                    if (ex is InvalidChoiceException)
                    {
                        showMessage(ex.Message);
                    }
                }

                if ((bool)isCoinValid)
                {
                    i_VendingMachine.InsertCoin((eCoin)clientCoin);
                }

                if (!i_VendingMachine.isSufficientFunds())
                {
                    showAmountLeft((int)i_VendingMachine.CurrentProduct - i_VendingMachine.CurrentBalance);
                }
            }
            while (!(bool)isCoinValid || !i_VendingMachine.isSufficientFunds());
        }

        private static void confirmPurchaseOrRefund(NewVendingMachine i_VendingMachine)
        {
            int? choice = null;
            bool isChoiceValid;
            eConfirmOrRefund userChoice;

            do
            {
                try
                {
                    choice = clientConfirmPurchaseOrRefund();
                    isChoiceValid = Enum.IsDefined(typeof(eConfirmOrRefund), choice);

                    if (!isChoiceValid)
                    {
                        throw new InvalidChoiceException();
                    }
                }
                catch (InvalidChoiceException invalidEx)
                {
                    isChoiceValid = false;
                    showMessage(invalidEx.Message);
                }
            }
            while (!isChoiceValid);

            userChoice = (eConfirmOrRefund)choice;
            i_VendingMachine.userChoiceSwitchCase(userChoice);
        }

        private static int selectProductAndGetPrice(NewVendingMachine i_VendingMachine, eProduct i_Product)
        {
            bool isAvailable = true;

            do
            {
                try
                {
                    if (i_VendingMachine.ProductInventory.getQuantity(i_Product) > 0)
                    {
                        i_VendingMachine.CurrentProduct = i_Product;
                    }
                    else
                    {
                        isAvailable = false;
                        throw new SoldOutException();
                    }
                }
                catch (SoldOutException SoldOut)
                {
                    UIInterface.PrintToCMD(SoldOut.Message);
                }
            }
            while (!isAvailable);

            return (int)i_Product;
        }

        private static int clientConfirmPurchaseOrRefund()
        {
            string userInput;

            string message = string.Format(
 @"Choose 'Confirm' to complete the purchase or 'Refund' to cancel:{0}
 1. Confirm{0}
 2. Refund{0}",
 Environment.NewLine);

            UIInterface.PrintToCMD(message);
            userInput = UIInterface.GetInputFromUser();

            return int.Parse(userInput);
        }

        private static void showAmountLeft(int i_AmountLeft)
        {
            string message = string.Format(
@"You need {0} more shekels to complete the purchase{1}",
                i_AmountLeft,
                Environment.NewLine);

            UIInterface.PrintToCMD(message);
        }

        private static string getCoinFromUser()
        {
            string coinFromClient = string.Empty;
            string message = string.Format(
@"Please enter coin:{0}",
                Environment.NewLine);

            UIInterface.PrintToCMD(message);
            coinFromClient = UIInterface.GetInputFromUser();

            return coinFromClient;
        }

        private static void enterAmountMessage(int i_Amount)
        {
            string message = string.Format(
@"Please enter {1} Shekels:{0} ",
                Environment.NewLine,
                i_Amount);

            UIInterface.PrintToCMD(message);
        }

        private static void invalidMessage()
        {
            string message = string.Format(
@"Invalid Input!{0}",
                Environment.NewLine);

            UIInterface.PrintToCMD(message);
        }

        private static void showMessage(string i_message)
        {
            UIInterface.PrintToCMD(i_message);
        }

        private static void showStatistics()
        {
            string message = string.Empty;

            MachineStatistics statistics = new MachineStatistics();

            message = statistics.StringifyStatistics();

            showMessage(message);
        }
    }
}