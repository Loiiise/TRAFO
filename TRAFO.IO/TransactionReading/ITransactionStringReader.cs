namespace TRAFO.IO.TransactionReading;

public interface ITransactionStringReader
{
    public string ReadNextLine(string source);
    public IEnumerable<string> ReadAllLines(string source);
}
