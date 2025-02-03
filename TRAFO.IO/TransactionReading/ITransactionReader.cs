using TRAFO.Logic;

namespace TRAFO.IO.TransactionReading;

public interface ITransactionReader
{
    public IEnumerable<Transaction> ReadAllTransactions();
}
