namespace VendingMachineController
{
    using System;
    using CMDInterface;
    using VendingMachine;
    using VendingMachine.Exceptions;

    /// <summary>
    /// Class for controlling the vending machine.
    /// </summary>
    public class Controller
    {
        // counter for refilling the products and change.
        private const int refillCounter = 10;

        public static void Run()
        {
            // Create new vending machine.
            NewVendingMachine vendingMachine = VendingMachineFactory.CreateNewVendingMachine();

            // Initialize vending machine.
            initializeMachine(vendingMachine);
        }

        /// <summary>
        /// Initializes the vending machine.
        /// </summary>
        /// <param name="i_VendingMachine">New vending machinme instance.</param>
        private static void initializeMachine(NewVendingMachine i_VendingMachine)
        {
            int price;

            i_VendingMachine.RefillAll(refillCounter);

            while (true)
            {
                // "Coke" Pre Selected               
                price = selectProductAndGetPrice(i_VendingMachine, eProduct.COKE);

                // Client is prompted to enter the selected product's price.
                enterAmountMessage(price);

                // Get money from client.
                recieveMoney(i_VendingMachine);

                // Client chooses to complete the purchase, or get the money back.
                confirmPurchaseOrRefund(i_VendingMachine);
            }
        }

        /// <summary>
        /// Receives the client's choice
        /// and retrieves the product's price.
        /// </summary>
        /// <returns>Price of selected product.</returns>
        /// <exception cref="VendingMachine.Exceptions.SoldOutException">Thrown when product is sold out.</exception>
        /// <param name="i_VendingMachine">Vending machine instance.</param>
        /// <param name="i_Product">Selected product.</param>
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

        /// <summary>
        /// Receives the client's money
        /// </summary>
        /// <exception cref="System.FormatException">Thrown when unexpected error occurs.</exception>
        /// <exception cref="VendingMachine.Exceptions.InvalidCoinException">Thrown when coin is invalid.</exception>
        /// <exception cref="VendingMachine.Exceptions.InvalidChoiceException">Thrown when choice is invalid.</exception>
        /// <param name="i_VendingMachine">Vending machine instance.</param>
        private static void recieveMoney(NewVendingMachine i_VendingMachine)
        {
            int? clientCoin = null;
            bool? isCoinValid = null;

            do
            {
                try
                {
                    // Getting money from client.
                    clientCoin = int.Parse(getCoinFromUser());

                    // Validating coin.
                    isCoinValid = Enum.IsDefined(typeof(eCoin), clientCoin);

                    if (!(bool)isCoinValid)
                    {
                        throw new InvalidCoinException();
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

                // Coin is valid, getting coin.
                if ((bool)isCoinValid)
                {
                    i_VendingMachine.InsertCoin((eCoin)clientCoin);
                }

                // Client gets prompted to insert the amount left for the chosen product.
                if (!i_VendingMachine.isSufficientFunds())
                {
                    showAmountLeft((int)i_VendingMachine.CurrentProduct - i_VendingMachine.CurrentBalance);
                }
            }
            while (!(bool)isCoinValid || !i_VendingMachine.isSufficientFunds());
        }

        /// <summary>
        /// confirms the client's purchase, or gives back the money.
        /// </summary>
        /// <exception cref="VendingMachine.Exceptions.InvalidChoiceException">Thrown when choice is invalid.</exception>
        /// <param name="i_VendingMachine">Vending machine instance.</param>
        private static void confirmPurchaseOrRefund(NewVendingMachine i_VendingMachine)
        {
            int? choice = null;
            bool isChoiceValid;
            eConfirmOrRefund userChoice;

            do
            {
                try
                {
                    // Getting client's choice.
                    choice = clientConfirmPurchaseOrRefund();

                    // Validating client's choice.
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

            // Running client's choice.
            userChoice = (eConfirmOrRefund)choice;
            i_VendingMachine.userChoiceSwitchCase(userChoice);
        }

        /// <summary>
        /// Prompts the client to confirm the purchase or get a refund.
        /// </summary>
        /// <returns>Correlating int input.</returns>
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

        /// <summary>
        /// Prompts the client of the amount left to pay.
        /// </summary>
        /// <param name="i_AmountLeft">Amount left to pay by the client.</param>
        private static void showAmountLeft(int i_AmountLeft)
        {
            string message = string.Format(
@"You need {0} more shekels to complete the purchase{1}",
                i_AmountLeft,
                Environment.NewLine);

            UIInterface.PrintToCMD(message);
        }

        /// <summary>
        /// Prompts the client to enter a coin.
        /// </summary>
        /// <returns>String representation of the client's coin.</returns>
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

        /// <summary>
        /// Prompts the client to enter specific amount of money.
        /// </summary>
        /// <param name="i_Amount">Amount to pay by the client.</param>
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