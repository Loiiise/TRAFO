namespace TRAFO.Logic.Categorization.Predicates;

public abstract record TransactionPredicate
{
    public abstract bool IsValid(Transaction transaction);
    public required string LabelToSet { get; init; }
}
