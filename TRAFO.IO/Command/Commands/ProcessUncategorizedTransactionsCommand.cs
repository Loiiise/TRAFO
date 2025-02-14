using TRAFO.IO.TransactionReading;
using TRAFO.IO.TransactionWriting;

namespace TRAFO.IO.Command;

public class ProcessUncategorizedTransactionsCommand : NoArgumentCommand
{
    public ProcessUncategorizedTransactionsCommand(
        IBasicUserInputHandler userInputHandler,
        IBasicUserOutputHandler outputHandler,
        ITransactionReader transactionReader,
        ITransactionLabelUpdater transactionLabelUpdater)
    {
        _userInputHandler = userInputHandler;
        _outputHandler = outputHandler;
        _transactionReader = transactionReader;
        _transactionLabelUpdater = transactionLabelUpdater;
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }

    private readonly IBasicUserInputHandler _userInputHandler;
    private readonly IBasicUserOutputHandler _outputHandler;
    private readonly ITransactionReader _transactionReader;
    private readonly ITransactionLabelUpdater _transactionLabelUpdater;
}
