namespace VendingMachine
{
    public class VendingMachineFactory
    {
        public static NewVendingMachine CreateNewVendingMachine()
        {
            return new NewVendingMachine();
        }
    }
}
