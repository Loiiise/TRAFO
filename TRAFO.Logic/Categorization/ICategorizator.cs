using TRAFO.Logic.Categorization.Predicates;

namespace TRAFO.Logic.Categorization;

public interface ICategorizator
{
    public Transaction ApplyPredicates(Transaction transaction, TransactionPredicate[] predicates);
    public IEnumerable<Transaction> ApplyPredicates(IEnumerable<Transaction> transactions, TransactionPredicate[] predicates);
}
