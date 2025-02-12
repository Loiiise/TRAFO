namespace TRAFO.Logic.Categorization;

public record TransactionPredicate
{
    public required Func<Transaction, bool> IsValid { get; init; }
    public required string LabelToSet { get; init; }
}
