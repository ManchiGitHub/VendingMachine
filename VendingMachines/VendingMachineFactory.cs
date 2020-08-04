namespace VendingMachine
{
    // Future types of vending machines goes here.
    public class VendingMachineFactory
    {
        public static NewVendingMachine CreateNewVendingMachine()
        {
            return new NewVendingMachine();
        }
    }
}
