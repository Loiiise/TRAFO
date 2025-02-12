
using TRAFO.Logic.Extensions;

namespace TRAFO.Logic.Categorization;

public class PrimairyLabelSetter : ICategorizator
{
    public IEnumerable<Transaction> ApplyPredicates(IEnumerable<Transaction> transactions, Predicate[] predicates)
    {
        foreach (var transaction in transactions)
        {
            var transacionHasBeenLabeled = false;

            foreach (var predicate in predicates)
            {
                if (predicate.IsValid(transaction))
                {
                    yield return transaction.SetPrimairyLabel(predicate.LabelToSet);
                    transacionHasBeenLabeled = true;
                    continue;
                }
            }

            if (!transacionHasBeenLabeled)
            {
                yield return transaction;
            }
        }
    }
}
