using System.Diagnostics.CodeAnalysis;
using TRAFO.Logic;

namespace TRAFO.Services.Parser;

public abstract class Parser : IParser
{
    public Transaction Parse(string line)
    {
        if (ParseSafe(line, out var transaction, out var exception))
        {
            return transaction!;
        }
        throw exception;
    }
    public IEnumerable<Transaction> Parse(IEnumerable<string> lines) => lines.Select(Parse);

    public bool TryParse(string line, [MaybeNullWhen(false)] out Transaction? transaction)
        => ParseSafe(line, out transaction, out var _);
    public IEnumerable<(bool, Transaction?)> TryParse(IEnumerable<string> lines)
        => lines.Select(line => (TryParse(line, out var transaction), transaction));

    protected abstract bool ParseSafe(string line, [MaybeNullWhen(false)] out Transaction? transaction, [MaybeNullWhen(true)] out Exception exception);
}