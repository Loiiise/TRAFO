using TRAFO.IO.Command.Flags;
using TRAFO.IO.TransactionReading;

namespace TRAFO.IO.Command;

public class ReportCommand : FromTillNoArgumentCommand
{
    public ReportCommand(ITransactionReader transactionReader, IBasicUserOutputHandler outputHandler, ICommandFlag[] flags) : base(flags)
    {
        _transactionReader = transactionReader;
        _outputHandler = outputHandler;
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }

    private readonly ITransactionReader _transactionReader;
    private readonly IBasicUserOutputHandler _outputHandler;
}
