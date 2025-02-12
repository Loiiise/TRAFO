namespace TRAFO.Logic.Categorization.Predicates;

public sealed record ContainsDescriptionPredicate : ContainsFieldPredicate
{
    protected override string? GetFieldFromTransaction(Transaction transaction) => transaction.Description;
}
