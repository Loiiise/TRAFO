using TRAFO.Logic.Dto;
using TRAFO.Repositories.Entities;

namespace TRAFO.Repositories;

public sealed class TransactionRepository : EntityFrameworkDatabase, ITransactionRepository
{
    private IQueryable<TransactionDatabaseEntry> QueryTransactionsInRange(DateTime? from, DateTime? till) => QueryTransactionsInRange(_context.Transaction, from, till);
    private IQueryable<TransactionDatabaseEntry> QueryTransactionsInRange(IQueryable<TransactionDatabaseEntry> baseQuery, DateTime? from, DateTime? till)
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
            .Where(t => t.ThisPartyAccount.AccountId == accountId)
            .ToDto();

    public IEnumerable<Transaction> ReadTransactionsFromLabel(Label label, bool includeParentTransactions = true) => ReadTransactionsFromLabel(label, null, null, includeParentTransactions);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(Label label, DateTime? from, DateTime? till, bool includeParentTransactions = true) => ReadTransactionsFromLabel(label.LabelId, from, till, includeParentTransactions);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(Guid labelId, bool includeParentTransactions = true) => ReadTransactionsFromLabel(labelId, null, null, includeParentTransactions);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(Guid labelId, DateTime? from, DateTime? till, bool includeParentTransactions = true)
        => QueryTransactionsInRange(from, till)
            .Where(t => t.Labels.Any(labelTransactionEntry => labelTransactionEntry.Label.LabelId == labelId))
            .WhereIf(!includeParentTransactions, childTransaction => !_context.Transaction.Any(t => t.ParentTransaction != null && t.ParentTransaction.TransactionId == childTransaction.TransactionId))
            .ToDto();

    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Label> labels) => ReadTransactionsFromLabel(labels, null, null);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Label> labels, DateTime? from, DateTime? till) => ReadTransactionsFromLabel(labels.Select(label => label.LabelId), from, till);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Guid> labelIds) => ReadTransactionsFromLabel(labelIds, null, null);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Guid> labelIds, DateTime? from, DateTime? till)
        => QueryTransactionsInRange(from, till)
            .Where(t => t.Labels.Any(labelTransactionEntry => labelIds.Contains(labelTransactionEntry.Label.LabelId)))
            .WhereIf(labelIds.Count() > 1, childTransaction => !_context.Transaction.Any(t => t.ParentTransaction != null && t.ParentTransaction.TransactionId == childTransaction.TransactionId))
            .ToDto();

    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(LabelCategory labelCategory) => ReadTransactionsFromLabelCategory(labelCategory, null, null);
    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(LabelCategory labelCategory, DateTime? from, DateTime? till) => ReadTransactionsFromLabelCategory(labelCategory.LabelCategoryId, from, till);
    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(Guid labelCategoryId) => ReadTransactionsFromLabelCategory(labelCategoryId, null, null);
    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(Guid labelCategoryId, DateTime? from, DateTime? till)
    {
        var labelIdsInCategory = _context.Label
            .Where(label => label.LabelCategory.LabelCategoryId == labelCategoryId)
            .Select(label => label.LabelId);

        return QueryTransactionsInRange(from, till)
             .Where(t => t.Labels.Any(label => labelIdsInCategory.Contains(label.Label.LabelId)))
             .ToDto();
    }

    public void WriteTransaction(Transaction transaction) => WriteTransactions(new[] { transaction });
    public void WriteTransactions(IEnumerable<Transaction> transactions)
    {
        // todo #88: make sure all entities exist (accounts and labels and such)
        //_context.Account.FirstOrDefault(account => account.AccountId == transaction.ThisAccountIdentifier) ?? throw new ArgumentException($"{transaction.ThisAccountIdentifier} does not exist"),
        var transactionsToWrite = new List<TransactionDatabaseEntry>();//transactions.Select(ToDatabaseEntry);
        _context.Transaction.AddRange(transactionsToWrite);
        // todo #88: link labels and transacions
        _context.SaveChanges();
    }

    internal static Transaction FromDatabaseEntry(TransactionDatabaseEntry transaction)
    {
        return new Transaction
        {
            TransactionId = transaction.TransactionId,
            Amount = transaction.Amount,
            Currency = transaction.ThisPartyAccount.Currency,
            ThisAccountIdentifier = transaction.ThisPartyAccount.AccountId,
            ThisAccountName = transaction.ThisPartyAccount.AccountName ?? transaction.ThisPartyAccount.AccountId,
            OtherAccountIdentifier = transaction.OtherPartyAccount.AccountId,
            OtherAccountName = transaction.OtherPartyAccount.AccountName ?? transaction.OtherPartyAccount.AccountId,
            Timestamp = transaction.Timestamp,
            PaymentReference = transaction.PaymentReference,
            BIC = transaction.BIC,
            Description = transaction.Description,
            RawData = GetRawData(transaction),
            Labels = transaction.Labels.Select(l => l.Label.Name).ToArray(),
        };

        string GetRawData(TransactionDatabaseEntry transaction)
            => transaction.RawData ??
                (transaction.ParentTransaction != null ?
                    GetRawData(transaction.ParentTransaction) :
                    string.Empty);
    }

    private TransactionDatabaseEntry ToDatabaseEntry(Transaction transaction, AccountDatabaseEntry thisPartyAccount, AccountDatabaseEntry otherPartyAccount)
    {
        return new TransactionDatabaseEntry
        {
            TransactionId = transaction.TransactionId,
            ParentTransaction = null,
            Amount = transaction.Amount,
            ThisPartyAccount = thisPartyAccount,
            OtherPartyAccount = otherPartyAccount,
            Timestamp = transaction.Timestamp,
            PaymentReference = transaction.PaymentReference,
            BIC = transaction.BIC,
            Description = transaction.Description,
            RawData = transaction.RawData,
        };
    }
}
