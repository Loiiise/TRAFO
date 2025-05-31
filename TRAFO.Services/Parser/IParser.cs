using TRAFO.Logic.Dto;

namespace TRAFO.Services.Parser;

public interface IParser
{
    public Transaction Parse(string line);
    public IEnumerable<Transaction> Parse(IEnumerable<string> lines);

    public bool TryParse(string line, out Transaction? transaction);
    public IEnumerable<(bool, Transaction?)> TryParse(IEnumerable<string> lines);
}