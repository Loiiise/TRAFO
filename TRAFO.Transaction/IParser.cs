using TRAFO.Logic;

namespace TRAFO.Parsing;
public interface IParser
{
    public Transaction Parse(string line);
    public IEnumerable<Transaction> Parse(IEnumerable<string> lines) => lines.Select(Parse);

    public bool TryParse(string line, out Transaction? transaction);
}
