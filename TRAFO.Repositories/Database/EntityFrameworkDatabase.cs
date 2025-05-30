using Microsoft.EntityFrameworkCore;
using TRAFO.Logic;
using TRAFO.Logic.Categorization;
using TRAFO.Repositories.Database.Entities;

namespace TRAFO.Repositories.Database;

public class EntityFrameworkDatabase : IDatabase
{
    public EntityFrameworkDatabase() : this(new DbContextOptions<EntityFrameworkDatabaseContext>()) { }
    public EntityFrameworkDatabase(DbContextOptions databaseContextOptions)
    {
        _context = new EntityFrameworkDatabaseContext(databaseContextOptions);
    }

    public IEnumerable<string> GetAllLabels() => new string[]
    {
        Labels.Groceries,
        Labels.Rent,
    };

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

    public IEnumerable<Balance> ReadBalances(string identifier)
    {
        // todo #77
        throw new NotImplementedException();
        //return _context.Balances.Where(b => b. == identifier).Select(FromDatabaseEntry);
    }

    public void WriteBalance(Balance balance)
    {
        // todo #77
        throw new NotImplementedException();
        /*
        _context.Balance.Add(ToDatabaseEntry(balance));
        _context.SaveChanges();
        */
    }

    // todo #72
    public void UpdatePrimairyLabel(Transaction transaction, string newPrimairyLabel) => UpdatePrimairyLabel(transaction with { PrimairyLabel = newPrimairyLabel });
    public void UpdatePrimairyLabel(Transaction transaction)
    {
        // todo #72
        throw new NotImplementedException();
        /*
        if (transaction.PrimairyLabel is null) return;

        var matches = _context.Transactions.Where(t => t.RawData == transaction.RawData);

        if (!matches.Any())
        {
            WriteTransaction(transaction);
            return;
        }
        Debug.Assert(matches.Count() == 1);

        var match = matches.First()!;
        // todo: #72
        //match.PrimairyLabel = transaction.PrimairyLabel;
        _context.Transactions.Update(match);
        _context.SaveChanges();
        */
    }

    public void UpdateLabels(Transaction transaction)
    {
        // todo #72 or #77
        throw new NotImplementedException();
    }

    private BalanceDatabaseEntry ToDatabaseEntry(Balance balance)
    {
        // todo #77
        throw new NotImplementedException();
        /*

        return new BalanceDatabaseEntry
        {
            Amount = balance.Amount,
            Currency = balance.Currency,
            ThisPartyIdentifier = balance.ThisPartyIdentifier,
            Timestamp = balance.Timestamp,
        };
        */
    }

    private TransacionDatabaseEntry ToDatabaseEntry(Transaction transaction)
    {
        // todo #77
        throw new NotImplementedException();
        /*
        return new TransacionDatabaseEntry
        {
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            ThisPartyIdentifier = transaction.ThisPartyIdentifier,
            ThisPartyName = transaction.ThisPartyName,
            OtherPartyIdentifier = transaction.OtherPartyIdentifier,
            OtherPartyName = transaction.OtherPartyName,
            Timestamp = transaction.Timestamp,
            PaymentReference = transaction.PaymentReference ?? string.Empty,
            BIC = transaction.BIC ?? string.Empty,
            Description = transaction.Description ?? string.Empty,
            RawData = transaction.RawData,
            PrimairyLabel = transaction.PrimairyLabel ?? string.Empty,
        };
        */
    }

    private Balance FromDatabaseEntry(BalanceDatabaseEntry balance)
    {
        // todo #77
        throw new NotImplementedException();
        /*
        return new Balance
        {
            Amount = balance.Amount,
            Currency = balance.Currency,
            ThisPartyIdentifier = balance.ThisPartyIdentifier,
            Timestamp = balance.Timestamp,
        };
        */
    }

    private Transaction FromDatabaseEntry(TransacionDatabaseEntry transaction)
    {
        // todo #77
        throw new NotImplementedException();
        /*
        Debug.Assert(account.AccountId == transaction.ThisPartyAccountId);

        return new Transaction
        {
            Amount = transaction.Amount,
            Currency = account.Currency,
            ThisPartyIdentifier = transaction.ThisPartyAccountId,
            ThisPartyName = transaction.ThisPartyName ?? transaction.ThisPartyAccountId,
            OtherPartyIdentifier = transaction.OtherPartyAccountId,
            OtherPartyName = transaction.OtherPartyName ?? transaction.OtherPartyAccountId,
            Timestamp = transaction.Timestamp,
            PaymentReference = transaction.PaymentReference,
            BIC = transaction.BIC,
            Description = transaction.Description,
            RawData = transaction.RawData ?? string.Empty,
            PrimairyLabel = string.Empty, // todo #:
            Labels = Array.Empty<string>(),
        };

        string? NullIfEmptyString(string value) => value == string.Empty ? null : value;

        */
    }

    private EntityFrameworkDatabaseContext _context;
}

internal class EntityFrameworkDatabaseContext : DbContext
{
    public EntityFrameworkDatabaseContext() : this(new DbContextOptions<EntityFrameworkDatabaseContext>()) { }
    public EntityFrameworkDatabaseContext(DbContextOptions options) : base(options)
    {
        var appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var programFolder = Path.Join(appdataFolder, "TRAFO"); // todo: #39

        if (!Directory.Exists(programFolder))
        {
            Directory.CreateDirectory(programFolder);
        }

        _databasePath = Path.Join(programFolder, "dummyDastabase.db"); // todo: #39
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (options.IsConfigured)
        {
            return;
        }
        options.UseSqlite($"Data Source={_databasePath}");
    }

    private string _databasePath { get; }
    public DbSet<AccountBalanceDatabaseEntry> Account { get; set; }
    public DbSet<AccountBalanceDatabaseEntry> AccountBalance { get; set; }
    public DbSet<LabelBalanceDatabaseEntry> LabelBalance { get; set; }
    public DbSet<LabelCategoryBalanceDatabaseEntry> LabelCategoryBalance { get; set; }
    public DbSet<LabelDatabaseEntry> Label { get; set; }
    public DbSet<TransacionDatabaseEntry> Transaction { get; set; }
    public DbSet<LabelCategoryDatabaseEntry> LabelCategorie { get; set; }
}
