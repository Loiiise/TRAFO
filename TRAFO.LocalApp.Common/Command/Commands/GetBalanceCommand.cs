using TRAFO.LocalApp.Common.Command.Arguments;
using TRAFO.LocalApp.Common.Command.Flags;
using TRAFO.Repositories.Interfaces;

namespace TRAFO.LocalApp.Common.Command;
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
