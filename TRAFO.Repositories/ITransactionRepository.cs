using TRAFO.Logic.Dto;

namespace TRAFO.Repositories;
internal interface ITransactionRepository :
    ITransactionReader,
    ITransactionWriter
{ }

public interface ITransactionReader
{
    public IEnumerable<Transaction> ReadTransactions();
    public IEnumerable<Transaction> ReadTransactions(DateTime? from, DateTime? till);

    public IEnumerable<Transaction> ReadTransactionsFromAccount(Account account);
    public IEnumerable<Transaction> ReadTransactionsFromAccount(Account account, DateTime? from, DateTime? till);
    public IEnumerable<Transaction> ReadTransactionsFromAccount(Guid accountId);
    public IEnumerable<Transaction> ReadTransactionsFromAccount(Guid accountId, DateTime? from, DateTime? till);

    public IEnumerable<Transaction> ReadTransactionsFromLabel(Label label, bool includeChildTransactions = true);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(Label label, DateTime? from, DateTime? till, bool includeChildTransactions = true);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(Guid labelId, bool includeChildTransactions = true);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(Guid labelId, DateTime? from, DateTime? till, bool includeChildTransactions = true);

    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Label> labels);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Label> labels, DateTime? from, DateTime? till);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Guid> labelIds);
    public IEnumerable<Transaction> ReadTransactionsFromLabel(IEnumerable<Guid> labelIds, DateTime? from, DateTime? till);

    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(LabelCategory labelCategory);
    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(LabelCategory labelCategory, DateTime? from, DateTime? till);
    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(Guid labelCategoryId);
    public IEnumerable<Transaction> ReadTransactionsFromLabelCategory(Guid labelCategoryId, DateTime? from, DateTime? till);
}

public interface ITransactionWriter
{
    public void WriteTransaction(Transaction transaction);
    public void WriteTransactions(IEnumerable<Transaction> transactions);
}
