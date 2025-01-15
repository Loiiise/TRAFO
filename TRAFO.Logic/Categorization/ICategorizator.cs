namespace TRAFO.Logic.Categorization;

public interface ICategorizator
{
    public void ApplyPredicates(IEnumerable<Transaction> transactions, Func<Transaction, Transaction>[] predicates);
}

public interface IAutomatedCategorizator : ICategorizator { }
public interface IUserInputCategorizator : ICategorizator { }