using System.Text;
using TRAFO.LocalApp.Common.Command.Flags;
using TRAFO.Logic.Dto;
using TRAFO.Repositories;

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

        _indexToLabel = new();
        var labelStringBuilder = new StringBuilder();
        int i = 0;
        foreach (var label in _labelReader.GetAllLabels().Prepend(new Label { Name = "Skip", }))
        {
            _indexToLabel.Add(i, label);
            labelStringBuilder.AppendLine($"[{i++}]: {label}");
        }
        _allCategoriesString = labelStringBuilder.ToString();
    }

    public override void Execute()
    {
        // todo #80
        var uncategorizedTransactions =
            _transactionReader
                .ReadTransactions(_from, _till)
                /*.Where(t => t.PrimairyLabel == null)*/;

        foreach (var uncategorizedTransaction in uncategorizedTransactions)
        {
            var labelIndex = _userInputHandler.GetNumericUserInput(
                "What label do you want add to this transaction?" + Environment.NewLine +
                uncategorizedTransaction.ToString() + Environment.NewLine +
                _allCategoriesString,
                0, _indexToLabel.Count());

            // 0 is skipping this transaction
            if (labelIndex != 0)
            {
                _transactionLabelUpdater.SetLabel(uncategorizedTransaction, _indexToLabel[labelIndex]);
            }
        }
    }

    private readonly IBasicUserInputHandler _userInputHandler;
    private readonly ILabelReader _labelReader;
    private readonly ITransactionReader _transactionReader;
    private readonly ITransactionLabelUpdater _transactionLabelUpdater;

    private readonly string _allCategoriesString;
    private readonly Dictionary<int, Label> _indexToLabel;
}
