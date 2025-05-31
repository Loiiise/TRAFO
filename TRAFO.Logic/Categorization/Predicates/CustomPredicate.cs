using TRAFO.Logic.Dto;

namespace TRAFO.Logic.Categorization.Predicates;

public record CustomPredicate : TransactionPredicate
{
    public required Func<Transaction, bool> Predicate { get; init; }

    public override bool IsValid(Transaction transaction) => Predicate(transaction);
}
