using TRAFO.Logic;

namespace TRAFO.IO.TransactionWriting;

public interface ITransactionWriter
{
    public void WriteTransaction(Transaction transaction);
    public void WriteTransactions(IEnumerable<Transaction> transactions);
}
