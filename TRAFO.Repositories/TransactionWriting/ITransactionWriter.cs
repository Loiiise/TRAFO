using TRAFO.Logic.Dto;

namespace TRAFO.Repositories.TransactionWriting;

public interface ITransactionWriter
{
    public void WriteTransaction(Transaction transaction);
    public void WriteTransactions(IEnumerable<Transaction> transactions);
}

public interface ITransactionLabelUpdater
{
    public void UpdateLabels(Transaction transaction);
}