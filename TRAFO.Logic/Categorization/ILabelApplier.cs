using TRAFO.Logic.Categorization.Predicates;
using TRAFO.Logic.Dto;

namespace TRAFO.Logic.Categorization;

public interface ILabelApplier
{
    public Transaction ApplyPredicates(Transaction transaction, TransactionPredicate[] predicates);
    public IEnumerable<Transaction> ApplyPredicates(IEnumerable<Transaction> transactions, TransactionPredicate[] predicates);
}
