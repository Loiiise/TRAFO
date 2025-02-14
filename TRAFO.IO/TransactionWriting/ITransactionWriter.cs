using TRAFO.Logic;

namespace TRAFO.IO.TransactionWriting;

public interface ITransactionWriter
{
    public void WriteTransaction(Transaction transaction);
    public void WriteTransactions(IEnumerable<Transaction> transactions);
}

public interface ITransactionLabelUpdater
{
    public void UpdatePrimairyLabel(Transaction transaction, string newPrimairyLabel);
    public void UpdatePrimairyLabel(Transaction transaction);
    public void UpdateLabels(Transaction transaction);
}