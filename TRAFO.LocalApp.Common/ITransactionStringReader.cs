namespace TRAFO.LocalApp.Common.TransactionReading;

public interface ITransactionStringReader
{
    public string ReadNextLine(string source);
    public IEnumerable<string> ReadAllLines(string source);
    public IEnumerable<string> ReadAllLines(string source, bool skipFirstLine);
}
