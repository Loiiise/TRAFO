using TRAFO.Logic;

namespace TRAFO.IO.TransactionWriting;

public interface ITransactionWriter
{
    public void WriteTransaction(Transaction transaction);
    public void WriteTransactions(IEnumerable<Transaction> transactions);
}

public interface ITransactionWriterToFile : ITransactionWriter { }
public interface ITransactionWriterToDatabase : ITransactionWriter { }
public interface ITransactionWriterToCommandline : ITransactionWriter { }