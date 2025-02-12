namespace TRAFO.Logic.Categorization;

public record Predicate
{
    public Predicate(Func<Transaction, bool> predicateFunction, string labelToSet)
    {
        IsValid = predicateFunction;
        LabelToSet = labelToSet;
    }

    public required Func<Transaction, bool> IsValid { get; init; }
    public required string LabelToSet { get; init; }
}
