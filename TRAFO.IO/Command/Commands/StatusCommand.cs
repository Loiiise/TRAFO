using TRAFO.IO.TransactionReading;

namespace TRAFO.IO.Command;
public class StatusCommand : NoArgumentCommand
{
    public StatusCommand(ITransactionReader transactionReader)
    {
        _transactionReader = transactionReader;
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }

    private readonly ITransactionReader _transactionReader;
}
