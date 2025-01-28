namespace TRAFO.Logic.Extensions;
public static class TransactionExtensions
{
    public static Transaction SetPrimairyLabel(this Transaction transaction, string primairyLabel, bool addToLabelCollection = true)
        => addToLabelCollection && !transaction.Labels.Contains(primairyLabel)
            ? transaction with
            {
                PrimairyLabel = primairyLabel,
                Labels = transaction.Labels.Prepend(primairyLabel).ToArray(),
            }
            : transaction with { PrimairyLabel = primairyLabel };

    public static Transaction RemovePrimairyLabel(this Transaction transaction, bool removeFromLabelCollection = true)
        => (transaction.PrimairyLabel != null && removeFromLabelCollection
            ? transaction.RemoveLabel(transaction.PrimairyLabel)
            : transaction)
        with
        { PrimairyLabel = null };

    public static Transaction AddLabel(this Transaction transaction, string label)
        => transaction.Labels.Contains(label)
            ? transaction
            : transaction with { Labels = transaction.Labels.Append(label).ToArray() };

    public static Transaction RemoveLabel(this Transaction transaction, string label)
        => transaction with { Labels = transaction.Labels.Where(l => l != label).ToArray() };

    public static Transaction RemoveAllLabels(this Transaction transaction, bool keepPrimairyLabel = false)
        => keepPrimairyLabel && transaction.PrimairyLabel != null
        ? transaction with { Labels = new string[] { transaction.PrimairyLabel } }
        : transaction with { Labels = Array.Empty<string>() };

}
