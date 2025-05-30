using System.Diagnostics;
using TRAFO.LocalApp.Common.Command.Flags;
using TRAFO.Logic;
using TRAFO.Repositories.TransactionReading;

namespace TRAFO.LocalApp.Common.Command;

public class ReportCommand : FromTillCommand
{
    public ReportCommand(ITransactionReader transactionReader, IBasicUserOutputHandler outputHandler, ICommandFlag[] flags) : base(flags)
    {
        _transactionReader = transactionReader;
        _outputHandler = outputHandler;
    }

    public override void Execute()
    {
        var noPrimairyLabel = "No category";

        var primairyLabelToGroupedTransactions = new Dictionary<string, List<Transaction>>();
        primairyLabelToGroupedTransactions[noPrimairyLabel] = new List<Transaction>();

        foreach (var transaction in _transactionReader.ReadTransactionsInRange(_from, _till))
        {
            if (transaction.PrimairyLabel is null)
            {
                primairyLabelToGroupedTransactions[noPrimairyLabel].Add(transaction);
                continue;
            }

            if (!primairyLabelToGroupedTransactions.ContainsKey(transaction.PrimairyLabel))
            {
                primairyLabelToGroupedTransactions[transaction.PrimairyLabel] = new List<Transaction> { transaction };
            }
            else
            {
                primairyLabelToGroupedTransactions[transaction.PrimairyLabel].Add(transaction);
            }
        }

        foreach ((var category, var transactions) in primairyLabelToGroupedTransactions)
        {
            if (!transactions.Any()) continue;
            var currency = transactions.First().Currency;

            Debug.Assert(transactions.All(t => t.Currency == currency));

            var totalSpendingInCategory = transactions.Sum(t => t.Amount);

            _outputHandler.GiveUserOutput($"{category}: {Transaction.ShowAmount(totalSpendingInCategory, currency)} {currency}");
        }
    }

    private readonly ITransactionReader _transactionReader;
    private readonly IBasicUserOutputHandler _outputHandler;
}
