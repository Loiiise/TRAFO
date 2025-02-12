namespace TRAFO.Logic.Categorization.Predicates;

public record DescriptionContainsPredicate : TransactionPredicate
{
    public required string ContainedString { get; init; }
    public bool CaseSensitive { get; init; } = false;
    public override bool IsValid(Transaction transaction)
        => transaction.Description != null
        && (CaseSensitive
            ? transaction.Description.Contains(ContainedString)
            : transaction.Description.ToLower().Contains(ContainedString.ToLower()));
}
