using TRAFO.IO.BalanceReading;
using TRAFO.IO.Command.Arguments;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;
public class GetBalanceCommand : Command
{
    public required IdentifierArgument IdentifierArgument { get; init; }

    public GetBalanceCommand(IBalanceReader balanceReader, ICommandFlag[] flags) : base(flags)
    {
        _balanceReader = balanceReader;
    }

    public override void Execute()
    {
        var momentToCheck = GetTimestampOrDefault() ?? DateTime.Now;
        _balanceReader.ReadBalances(IdentifierArgument.Value).Last(b => b.Timestamp <= momentToCheck);
    }

    private DateTime? GetTimestampOrDefault()
        => Flags.Any(f => f is DateFlag) ?
            ((DateFlag)Flags.First(f => f is DateFlag)).Value :
            null;

    private readonly IBalanceReader _balanceReader;
}
