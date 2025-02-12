namespace TRAFO.Logic.Categorization;

public interface ICategorizator
{
    public IEnumerable<Transaction> ApplyPredicates(IEnumerable<Transaction> transactions, Predicate[] predicates);
}
