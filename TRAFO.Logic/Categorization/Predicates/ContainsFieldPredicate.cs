namespace TRAFO.Logic.Categorization.Predicates;

public abstract record ContainsFieldPredicate : TransactionPredicate
{
    public required string ContainedString { get; init; }
    public bool CaseSensitive { get; init; } = false;
    public override bool IsValid(Transaction transaction) 
        => IsValid(GetFieldFromTransaction(transaction));
    private bool IsValid(string? field)
        => field != null
        && (CaseSensitive
            ? field.Contains(ContainedString)
            : field.ToLower().Contains(ContainedString.ToLower()));
    
    protected abstract string? GetFieldFromTransaction(Transaction transaction);
}
