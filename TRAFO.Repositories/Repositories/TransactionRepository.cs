using TRAFO.Logic.Dto;

namespace TRAFO.Repositories;
public sealed class TransactionRepository : EntityFrameworkDatabase, ITransactionRepository
{
    private IQueryable<Transaction> QueryTransactions() => _context.Transaction.Select(dbEntity => FromDatabaseEntry(dbEntity));
    private IQueryable<Transaction> QueryTransactionsInRange(DateTime? from, DateTime? till)
        => from == null && till == null ?
            QueryTransactions() :
            from == null ?
                QueryTransactions().Where(t => t.Timestamp <= till) :
                till == null ?
                    QueryTransactions().Where(t => t.Timestamp >= from) :
                    QueryTransactions()
                        .Where(t => t.Timestamp <= till)
                        .Where(t => t.Timestamp >= from);

    public IEnumerable<Transaction> ReadTransactions()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactions(DateTime? from, DateTime? till)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromAccount(Account account)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromAccount(Account account, DateTime? from, DateTime? till)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromAccount(Guid accountId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromAccount(Guid accountId, DateTime? from, DateTime? till)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabel(Label label, bool includeChildTransactions = true)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabel(Label label, DateTime? from, DateTime? till, bool includeChildTransactions = true)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabel(Guid labelId, bool includeChildTransactions = true)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabel(Guid labelId, DateTime? from, DateTime? till, bool includeChildTransactions = true)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Label> labels)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Label> labels, DateTime? from, DateTime? till)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Guid> labelIds)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Guid> labelIds, DateTime? from, DateTime? till)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(LabelCategory labelCategory)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(LabelCategory labelCategory, DateTime? from, DateTime? till)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(Guid labelCategoryId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(Guid labelCategoryId, DateTime? from, DateTime? till)
    {
        throw new NotImplementedException();
    }

    public void WriteTransaction(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public void WriteTransactions(IEnumerable<Transaction> transactions)
    {
        throw new NotImplementedException();
    }
}
