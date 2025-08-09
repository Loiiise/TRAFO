using TRAFO.Logic.Dto;

namespace TRAFO.Logic.Categorization.Predicates;

public sealed record ContainsOtherPartyNamePredicate : ContainsFieldPredicate
{
    protected override string? GetFieldFromTransaction(Transaction transaction) => transaction.OtherAccountName;
}
