using TRAFO.LocalApp.Common.Command.Flags;
using TRAFO.Repositories.Interfaces;

namespace TRAFO.LocalApp.Common.Command;

public class ShowUncategorizedTransactionsCommand : FromTillCommand
{
    public ShowUncategorizedTransactionsCommand(ITransactionReader transactionReader, IBasicUserOutputHandler userOutputHandler, ICommandFlag[] flags) : base(flags)
    {
        _transactionReader = transactionReader;
        _userOutputHandler = userOutputHandler;
    }

    public override void Execute()
    {
        var uncategorizedTransactions = _transactionReader
            .ReadTransactionsInRange(_from, _till)
            // todo #80
            //.Where(t => t.PrimairyLabel is null)
            .ToArray();

        var totalCount = uncategorizedTransactions.Length;

        if (totalCount == 0)
        {
            _userOutputHandler.GiveUserOutput("All transactions are sorted out. Good job!");
            return;
        }

        for (int i = 0; i < totalCount; ++i)
        {
            _userOutputHandler.GiveUserOutput($"({i}/{totalCount}) - {uncategorizedTransactions[i].ToString()}");
        }
    }

    private readonly ITransactionReader _transactionReader;
    private readonly IBasicUserOutputHandler _userOutputHandler;
}
