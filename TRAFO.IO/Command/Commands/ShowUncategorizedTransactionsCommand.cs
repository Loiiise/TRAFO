using TRAFO.IO.Command.Flags;
using TRAFO.IO.TransactionReading;

namespace TRAFO.IO.Command;

public class ShowUncategorizedTransactionsCommand : NoArgumentCommand
{
    public ShowUncategorizedTransactionsCommand(ITransactionReader transactionReader, IBasicUserOutputHandler userOutputHandler, ICommandFlag[] flags) : base(flags)
    {
        _transactionReader = transactionReader;
        _userOutputHandler = userOutputHandler;
    }

    public override void Execute()
    {
        var uncategorizedTransactions = _transactionReader
            .ReadAllTransactions()
            .Where(t => t.PrimairyLabel is null)
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

    protected override bool IsSupported(ICommandFlag flag)
    {
        throw new NotImplementedException();
    }

    private readonly ITransactionReader _transactionReader;
    private readonly IBasicUserOutputHandler _userOutputHandler;
}
