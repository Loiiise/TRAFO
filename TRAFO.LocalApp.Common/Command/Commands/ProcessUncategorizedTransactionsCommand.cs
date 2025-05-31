using System.Text;
using TRAFO.LocalApp.Common.Command.Flags;
using TRAFO.Repositories.TransactionReading;
using TRAFO.Repositories.TransactionWriting;

namespace TRAFO.LocalApp.Common.Command;

public class ProcessUncategorizedTransactionsCommand : FromTillCommand
{
    public ProcessUncategorizedTransactionsCommand(
        IBasicUserInputHandler userInputHandler,
        ILabelReader labelReader,
        ITransactionReader transactionReader,
        ITransactionLabelUpdater transactionLabelUpdater,
        ICommandFlag[] flags) : base(flags)
    {
        _userInputHandler = userInputHandler;
        _labelReader = labelReader;
        _transactionReader = transactionReader;
        _transactionLabelUpdater = transactionLabelUpdater;

        _indexToCategory = new();
        var labelStringBuilder = new StringBuilder();
        int i = 0;
        foreach (var label in _labelReader.GetAllLabels().Prepend("Skip"))
        {
            _indexToCategory.Add(i, label);
            labelStringBuilder.AppendLine($"[{i++}]: {label}");
        }
        _allCategoriesString = labelStringBuilder.ToString();
    }

    public override void Execute()
    {
        // todo #80
        var uncategorizedTransactions = 
            _transactionReader
                .ReadTransactionsInRange(_from, _till)
                /*.Where(t => t.PrimairyLabel == null)*/;

        foreach (var uncategorizedTransaction in uncategorizedTransactions)
        {
            var labelIndex = _userInputHandler.GetNumericUserInput(
                "What label do you want add to this transaction?" + Environment.NewLine +
                uncategorizedTransaction.ToString() + Environment.NewLine +
                _allCategoriesString,
                0, _indexToCategory.Count());

            // 0 is skipping this transaction
            if (labelIndex != 0)
            {
                _transactionLabelUpdater.UpdatePrimairyLabel(uncategorizedTransaction, _indexToCategory[labelIndex]);
            }
        }
    }

    private readonly IBasicUserInputHandler _userInputHandler;
    private readonly ILabelReader _labelReader;
    private readonly ITransactionReader _transactionReader;
    private readonly ITransactionLabelUpdater _transactionLabelUpdater;

    private readonly string _allCategoriesString;
    private readonly Dictionary<int, string> _indexToCategory;
}
