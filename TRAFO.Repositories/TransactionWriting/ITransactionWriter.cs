using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.TransactionWriting;

public interface ITransactionWriter
{
    public void WriteTransaction(Transaction transaction);
    public void WriteTransactions(IEnumerable<Transaction> transactions);
}

public interface ITransactionLabelUpdater
{
    public void SetLabel(Transaction transaction, Label label);
    public void UpdateLabels(Transaction transaction);
}