using TRAFO.IO.Command.Flags;
using TRAFO.IO.TransactionReading;

namespace TRAFO.IO.Command;
public class StatusCommand : FromTillNoArgumentCommand
{
    public StatusCommand(ITransactionReader transactionReader, IBasicUserOutputHandler userOutputHandler, ICommandFlag[] flags) : base(flags)
    {
        _transactionReader = transactionReader;
        _userOutputHandler = userOutputHandler;
    }

    public override void Execute()
    {
        var transactions = _transactionReader.ReadTransactionsInRange(_from, _till);

        if (!transactions.Any())
        {
            _userOutputHandler.GiveUserOutput("There are no transactions loaded in yet!");
            return;
        }

        var transactionCount = transactions.Count();
        var categorizedTransactionCount = transactions.Where(t => t.PrimairyLabel is not null).Count();
        var oldestUncategorized = transactions.Where(t => t.PrimairyLabel is null).MinBy(t => t.Timestamp)!;
        
        _userOutputHandler.GiveUserOutput($"You categorized {categorizedTransactionCount}/{transactionCount} transactions.");
        _userOutputHandler.GiveUserOutput($"The oldest uncategorized transaction is from {oldestUncategorized.Timestamp}");
    }

    private readonly ITransactionReader _transactionReader;
    private readonly IBasicUserOutputHandler _userOutputHandler;
}
