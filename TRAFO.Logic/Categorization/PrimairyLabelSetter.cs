
using TRAFO.Logic.Extensions;

namespace TRAFO.Logic.Categorization;

public class PrimairyLabelSetter : ICategorizator
{
    public Transaction ApplyPredicates(Transaction transaction, TransactionPredicate[] predicates)
    {
        foreach (var predicate in predicates)
        {
            if (predicate.IsValid(transaction))
            {
                return transaction.SetPrimairyLabel(predicate.LabelToSet);
            }
        }

        return transaction;        
    }

    public IEnumerable<Transaction> ApplyPredicates(IEnumerable<Transaction> transactions, TransactionPredicate[] predicates)
        => transactions.Select(t => ApplyPredicates(t, predicates));
}
