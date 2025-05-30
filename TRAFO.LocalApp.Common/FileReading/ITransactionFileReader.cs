using System.Diagnostics.CodeAnalysis;
using TRAFO.Logic;

namespace TRAFO.LocalApp.Common.FileReading;

public interface ITransactionFileReader
{
    public IEnumerable<Transaction> ReadAllTransactions(string path);
    public IEnumerable<Transaction> ReadAllTransactions(string path, TransactionFileReaderConfiguration configuration);

    public bool TryReadAllTransactions(string path, [NotNullWhen(true)] out IEnumerable<Transaction> transactions);
    public bool TryReadAllTransactions(string path, TransactionFileReaderConfiguration configuration, [NotNullWhen(true)] out IEnumerable<Transaction> transactions);
}
