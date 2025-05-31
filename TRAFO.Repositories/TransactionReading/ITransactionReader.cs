using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.TransactionReading;

public interface ITransactionReader
{
    public IEnumerable<Transaction> ReadAllTransactions();
    public IEnumerable<Transaction> ReadTransactionsInRange(DateTime? from, DateTime? till);
}
