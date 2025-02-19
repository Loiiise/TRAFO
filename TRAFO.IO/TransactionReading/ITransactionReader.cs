using TRAFO.Logic;

namespace TRAFO.IO.TransactionReading;

public interface ITransactionReader
{
    public IEnumerable<Transaction> ReadAllTransactions();
    public IEnumerable<Transaction> ReadTransactionsInRange(DateTime? from, DateTime? till);
}
