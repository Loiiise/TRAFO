using TRAFO.Logic.Dto;

namespace TRAFO.Logic.Extensions;
public static class TransactionExtensions
{
    public static Transaction AddLabel(this Transaction transaction, string label)
        => transaction.Labels.Contains(label)
            ? transaction
            : transaction with { Labels = transaction.Labels.Append(label).ToArray() };

    public static Transaction AddLabels(this Transaction transaction, params string[] labels) => AddLabels(transaction, labels.AsEnumerable());
    public static Transaction AddLabels(this Transaction transaction, IEnumerable<string> labels)
    {
        var newLabels = labels.Distinct().Where(l => !transaction.Labels.Contains(l));

        return newLabels.Any()
            ? transaction with { Labels = transaction.Labels.Concat(newLabels).ToArray() }
            : transaction;
    }

    public static Transaction RemoveLabel(this Transaction transaction, string label)
        => transaction with { Labels = transaction.Labels.Where(l => l != label).ToArray() };

    public static Transaction RemoveAllLabels(this Transaction transaction)
        => transaction with { Labels = Array.Empty<string>() };
}
