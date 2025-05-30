using TRAFO.Logic;

namespace TRAFO.Repositories.TransactionWriting;

public interface ITransactionWriter
{
    public void WriteTransaction(Transaction transaction);
    public void WriteTransactions(IEnumerable<Transaction> transactions);
}

public interface ITransactionLabelUpdater
{
    // todo #72
    public void UpdatePrimairyLabel(Transaction transaction, string newPrimairyLabel);
    public void UpdatePrimairyLabel(Transaction transaction);
    public void UpdateLabels(Transaction transaction);
}