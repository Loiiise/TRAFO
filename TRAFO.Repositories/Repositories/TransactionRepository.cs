using TRAFO.Logic.Dto;
using TRAFO.Repositories.Entities;

namespace TRAFO.Repositories;

public sealed class TransactionRepository : EntityFrameworkDatabase, ITransactionRepository
{
    private IQueryable<TransacionDatabaseEntry> QueryTransactionsInRange(DateTime? from, DateTime? till) => QueryTransactionsInRange(_context.Transaction, from, till);
    private IQueryable<TransacionDatabaseEntry> QueryTransactionsInRange(IQueryable<TransacionDatabaseEntry> baseQuery, DateTime? from, DateTime? till)
        => from == null && till == null ?
            baseQuery :
            from == null ?
                baseQuery.Where(t => t.Timestamp <= till) :
                till == null ?
                    baseQuery.Where(t => t.Timestamp >= from) :
                    baseQuery
                        .Where(t => t.Timestamp <= till)
                        .Where(t => t.Timestamp >= from);

    public IEnumerable<Transaction> ReadTransactions() => ReadTransactions(null, null);
    public IEnumerable<Transaction> ReadTransactions(DateTime? from, DateTime? till) => QueryTransactionsInRange(from, till).ToDto();

    public IEnumerable<Transaction> ReadTransactionsFromAccount(Account account) => ReadTransactionsFromAccount(account, null, null);
    public IEnumerable<Transaction> ReadTransactionsFromAccount(Account account, DateTime? from, DateTime? till) => ReadTransactionsFromAccount(account.AccountId, from, till);
    public IEnumerable<Transaction> ReadTransactionsFromAccount(string accountId) => ReadTransactionsFromAccount(accountId, null, null);
    public IEnumerable<Transaction> ReadTransactionsFromAccount(string accountId, DateTime? from, DateTime? till)
        => QueryTransactionsInRange(from, till)
            .Where(t => t.ThisPartyAccountId == accountId)
            .ToDto();

    public IEnumerable<Transaction> ReadTransactionsFromLabel(Label label, bool includeParentTransactions = true) => ReadTransactionsFromLabel(label, null, null, includeParentTransactions);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(Label label, DateTime? from, DateTime? till, bool includeParentTransactions = true) => ReadTransactionsFromLabel(label.LabelId, from, till, includeParentTransactions);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(Guid labelId, bool includeParentTransactions = true) => ReadTransactionsFromLabel(labelId, null, null, includeParentTransactions);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(Guid labelId, DateTime? from, DateTime? till, bool includeParentTransactions = true)
        => QueryTransactionsInRange(from, till)
            .Where(t => t.Labels.Any(labelTransactionEntry => labelTransactionEntry.LabelId == labelId))
            .WhereIf(!includeParentTransactions, childTransaction => !_context.Transaction.Any(t => t.ParentTransactionId == childTransaction.TransactionId))
            .ToDto();


    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Label> labels) => ReadTransactionsFromLabel(labels, null, null);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Label> labels, DateTime? from, DateTime? till) => ReadTransactionsFromLabel(labels.Select(label => label.LabelId), from, till);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Guid> labelIds) => ReadTransactionsFromLabel(labelIds, null, null);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Guid> labelIds, DateTime? from, DateTime? till)
        => QueryTransactionsInRange(from, till)
            .Where(t => t.Labels.Any(labelTransactionEntry => labelIds.Contains(labelTransactionEntry.LabelId)))
            .ToDto();

    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(LabelCategory labelCategory) => ReadTransactionsFromLabelCategory(labelCategory, null, null);
    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(LabelCategory labelCategory, DateTime? from, DateTime? till) => ReadTransactionsFromLabelCategory(labelCategory.LabelCategoryId, from, till);
    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(Guid labelCategoryId) => ReadTransactionsFromLabelCategory(labelCategoryId, null, null);
    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(Guid labelCategoryId, DateTime? from, DateTime? till)
    {
        var labelIdsInCategory = _context.Label
            .Where(label => label.LabelCategoryId == labelCategoryId)
            .Select(label => label.LabelId);

        return QueryTransactionsInRange(from, till)
             .Where(t => t.Labels.Any(label => labelIdsInCategory.Contains(label.LabelId)))
             .ToDto();
    }

    public void WriteTransaction(Transaction transaction)
    {
        _context.Transaction.Add(ToDatabaseEntry(transaction));
        _context.SaveChanges();
    }

    public void WriteTransactions(IEnumerable<Transaction> transactions)
    {
        _context.Transaction.AddRange(transactions.Select(ToDatabaseEntry));
        _context.SaveChanges();
    }
}
