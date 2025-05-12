using TRAFO.IO.BalanceWriting;
using TRAFO.IO.Command.Arguments;
using TRAFO.IO.Command.Flags;
using TRAFO.Logic;

namespace TRAFO.IO.Command.Commands;
public class SetBalanceCommand : Command
{
    public required AmountArgument AmountArgument { get; init; }
    public required CurrencyArgument CurrencyArgument { get; init; }
    public required IdentifierArgument IdentifierArgument { get; init; }

    public SetBalanceCommand(IBalanceWriter balanceWriter, ICommandFlag[] flags) : base(flags)
    {
        _balanceWriter = balanceWriter;
    }

    public override void Execute()
    {
        var balance = new Balance
        {
            Amount = AmountArgument.Value,
            Currency = CurrencyArgument.Value,
            ThisPartyIdentifier = IdentifierArgument.Value,
            Timestamp = GetTimestampOrDefault() ?? DateTime.Now,
        };

        _balanceWriter.WriteBalance(balance);
    }

    private DateTime? GetTimestampOrDefault()
        => Flags.Any(f => f is DateFlag) ?
            ((DateFlag)Flags.First(f => f is DateFlag)).Value :
            null;

    private readonly IBalanceWriter _balanceWriter;
}
