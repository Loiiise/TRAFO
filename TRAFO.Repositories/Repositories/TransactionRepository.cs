using TRAFO.Logic.Dto;

namespace TRAFO.Repositories;
public sealed class TransactionRepository : EntityFrameworkDatabase, ITransactionRepository
{
    public IEnumerable<Transaction> ReadAllTransactions()
    {
        return _context.Transaction.Select(FromDatabaseEntry);
    }

    public IEnumerable<Transaction> ReadTransactionsInRange(DateTime? from, DateTime? till)
    {
        var transactions = ReadAllTransactions();

        if (from is not null) transactions = transactions.Where(t => t.Timestamp >= from);
        if (till is not null) transactions = transactions.Where(t => t.Timestamp <= till);

        return transactions;
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
