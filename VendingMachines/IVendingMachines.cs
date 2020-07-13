namespace VendingMachine
{
    using System.Collections.Generic;

    public interface IVendingMachines
    {
        Bucket<List<eCoin>, eProduct> CollectChangeAndItem();

        List<eCoin> Refund();

        void ResetMachine();
    }
}
