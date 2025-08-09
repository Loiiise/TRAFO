using TRAFO.LocalApp.Common.Command.Flags;
using TRAFO.Repositories;

namespace TRAFO.LocalApp.Common.Command;
public class StatusCommand : FromTillCommand
{
    public StatusCommand(ITransactionReader transactionReader, IBasicUserOutputHandler userOutputHandler, ICommandFlag[] flags) : base(flags)
    {
        _transactionReader = transactionReader;
        _userOutputHandler = userOutputHandler;
    }

    public override void Execute()
    {
        var transactions = _transactionReader.ReadTransactions(_from, _till);

        if (!transactions.Any())
        {
            _userOutputHandler.GiveUserOutput("There are no transactions loaded in yet!");
            return;
        }

        var transactionCount = transactions.Count();
        var categorizedTransactionCount = transactions
            // todo #80
            //.Where(t => t.PrimairyLabel is not null)
            .Count();
        var oldestUncategorized = transactions
            // todo #80
            // .Where(t => t.PrimairyLabel is null)
            .MinBy(t => t.Timestamp)!;

        _userOutputHandler.GiveUserOutput($"You categorized {categorizedTransactionCount}/{transactionCount} transactions.");
        _userOutputHandler.GiveUserOutput($"The oldest uncategorized transaction is from {oldestUncategorized.Timestamp}");
    }

    private readonly ITransactionReader _transactionReader;
    private readonly IBasicUserOutputHandler _userOutputHandler;
}
