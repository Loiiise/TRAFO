namespace TRAFO.IO.TransactionReading;

public interface ITransactionStringReader
{
    public string ReadLine();
    public IEnumerable<string> ReadAllLines();
}

public interface ITransactionStringReaderFromFile : ITransactionStringReader { }
public interface ITransactionStringReaderFromDatabase : ITransactionStringReader { }
public interface ITransactionStringReaderFromCommandLine : ITransactionStringReader { }
