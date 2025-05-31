using TRAFO.Logic.Categorization.Predicates;
using TRAFO.Logic.Dto;
using TRAFO.Logic.Extensions;

namespace TRAFO.Logic.Categorization;

public class TransactionLabelSetter : ILabelApplier
{
    public Transaction ApplyPredicates(Transaction transaction, TransactionPredicate[] predicates)
        => transaction
            .AddLabels(predicates
                .Where(p => p.IsValid(transaction))
                .Select(p => p.LabelToSet));

    public IEnumerable<Transaction> ApplyPredicates(IEnumerable<Transaction> transactions, TransactionPredicate[] predicates)
        => transactions.Select(t => ApplyPredicates(t, predicates));
}
