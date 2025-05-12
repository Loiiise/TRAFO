using TRAFO.IO.BalanceWriting;
using TRAFO.IO.Command.Arguments;
using TRAFO.IO.Command.Flags;

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
        throw new NotImplementedException();
    }

    private readonly IBalanceWriter _balanceWriter;
}
