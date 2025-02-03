namespace TRAFO.IO.TransactionReading;

public class FileReader : ITransactionStringReader
{
    public string ReadNextLine(string source)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> ReadAllLines(string source) => ReadAllLines(source, false);
    public IEnumerable<string> ReadAllLines(string source, bool skipFirstLine) => skipFirstLine 
        ? File.ReadAllLines(source).Skip(1)
        : File.ReadAllLines(source);
}
