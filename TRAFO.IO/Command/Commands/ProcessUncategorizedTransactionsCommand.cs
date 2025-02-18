using System.Text;
using TRAFO.IO.Command.Flags;
using TRAFO.IO.TransactionReading;
using TRAFO.IO.TransactionWriting;

namespace TRAFO.IO.Command;

public class ProcessUncategorizedTransactionsCommand : FromTillNoArgumentCommand
{
    public ProcessUncategorizedTransactionsCommand(
        IBasicUserInputHandler userInputHandler,
        ICategoryReader categoryReader,
        ITransactionReader transactionReader,
        ITransactionLabelUpdater transactionLabelUpdater,
        ICommandFlag[] flags) : base(flags)
    {
        _userInputHandler = userInputHandler;
        _categoryReader = categoryReader;
        _transactionReader = transactionReader;
        _transactionLabelUpdater = transactionLabelUpdater;

        _indexToCategory = new();
        var categoryStringBuilder = new StringBuilder();
        int i = 0;
        foreach (var category in _categoryReader.GetAllCategories().Prepend("Skip"))
        {
            _indexToCategory.Add(i, category);
            categoryStringBuilder.AppendLine($"[{i++}]: {category}");
        }
        _allCategoriesString = categoryStringBuilder.ToString();
    }

    public override void Execute()
    {
        var uncategorizedTransactions = _transactionReader.ReadAllTransactions().Where(t => t.PrimairyLabel == null);

        foreach (var uncategorizedTransaction in uncategorizedTransactions)
        {
            var categoryIndex = _userInputHandler.GetNumericUserInput(
                "In what category do you want to place this transaction?" + Environment.NewLine +
                uncategorizedTransaction.ToString() + Environment.NewLine +
                _allCategoriesString,
                0, _indexToCategory.Count());

            // 0 is skipping this transaction
            if (categoryIndex != 0)
            {
                _transactionLabelUpdater.UpdatePrimairyLabel(uncategorizedTransaction, _indexToCategory[categoryIndex]);
            }
        }
    }

    private readonly IBasicUserInputHandler _userInputHandler;
    private readonly ICategoryReader _categoryReader;
    private readonly ITransactionReader _transactionReader;
    private readonly ITransactionLabelUpdater _transactionLabelUpdater;

    private readonly string _allCategoriesString;
    private readonly Dictionary<int, string> _indexToCategory;
}
