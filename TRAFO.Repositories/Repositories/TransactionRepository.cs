using TRAFO.Logic.Dto;
using TRAFO.Repositories.Entities;

namespace TRAFO.Repositories;

public sealed class TransactionRepository : EntityFrameworkDatabase, ITransactionRepository
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILabelRepository _labelRepository;

    public TransactionRepository(IAccountRepository accountRepository, ILabelRepository labelRepository) : base()
    {
        _accountRepository = accountRepository;
        _labelRepository = labelRepository;
    }

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
        var firstTransaction = transactions.FirstOrDefault();
        if (firstTransaction is null) return;

        if (transactions.Any(t => t.Currency != firstTransaction.Currency))
        {
            throw new ArgumentException("All transactions must have the same currency.");
        }

        var accountsDtos = transactions
            .Select(t => (t.ThisAccountIdentifier, t.ThisAccountName))
            .Concat(transactions.Select(t => (t.OtherAccountIdentifier, t.OtherAccountName)))
            .Distinct()
            .Select(thisAccountIdAndName => new Account
            {
                AccountId = thisAccountIdAndName.Item1,
                AccountName = thisAccountIdAndName.Item2,
                // Balance is unknown in transactions, so we set it to 0 at the start of time. 
                Balance = new Balance
                {
                    Amount = 0,
                    Currency = firstTransaction.Currency,
                    Timestamp = new DateTime(0),
                },
            });
        var labels = transactions.SelectMany(t => t.Labels).Distinct();

        _accountRepository.CreateIfNotExists(accountsDtos);
        _labelRepository.CreateIfNotExists(labels);
        _context.SaveChanges();

        // All related entities exist now, we can setup transactions
        var accountDatabaseEntriesById = _context.Account
            .Where(account => accountsDtos.Select(a => a.AccountId).Contains(account.AccountId))
            .ToDictionary(account => account.AccountId);
        var labelDatabaseEntriesByName = _context.Label
            .Where(label => labels.Contains(label.Name))
            .ToDictionary(label => label.Name);

        var transactionsToAddTuples = transactions.Select(transaction =>
            (ToDatabaseEntry(
                transaction,
                accountDatabaseEntriesById[transaction.ThisAccountIdentifier],
                accountDatabaseEntriesById[transaction.OtherAccountIdentifier])
            , transaction));

        _context.Transaction.AddRange(transactionsToAddTuples.Select(transactionTuple => transactionTuple.Item1));
        _context.TransactionLabels.AddRange(transactionsToAddTuples
            .SelectMany(transactionTuple => transactionTuple.Item2.Labels.Select(labelName =>
            new TransacionLabelerDatabaseEntry
            {
                Transaction = transactionTuple.Item1,
                Label = labelDatabaseEntriesByName[labelName],
            })));
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
