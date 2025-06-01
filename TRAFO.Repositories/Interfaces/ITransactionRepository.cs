using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.Interfaces;
internal interface ITransactionRepository :
    ITransactionReader,
    ITransactionWriter
{ }

public interface ITransactionReader
{
    public IEnumerable<Transaction> ReadAllTransactions();
    public IEnumerable<Transaction> ReadTransactionsInRange(DateTime? from, DateTime? till);
}

public interface ITransactionWriter
{
    public void WriteTransaction(Transaction transaction);
    public void WriteTransactions(IEnumerable<Transaction> transactions);
}
