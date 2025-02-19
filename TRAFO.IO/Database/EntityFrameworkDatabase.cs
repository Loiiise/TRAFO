using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TRAFO.Logic;
using TRAFO.Logic.Categorization;

namespace TRAFO.IO.Database;

public class EntityFrameworkDatabase : IDatabase
{
    public EntityFrameworkDatabase() : this(new DbContextOptions<EntityFrameworkDatabaseContext>()) { }
    public EntityFrameworkDatabase(DbContextOptions databaseContextOptions)
    {
        _context = new EntityFrameworkDatabaseContext(databaseContextOptions);
    }

    public IEnumerable<string> GetAllCategories() => Categories.GetAllCategories();

    public IEnumerable<Transaction> ReadAllTransactions()
    {
        return _context.Transactions.Select(FromDatabaseEntry);
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
        _context.Transactions.Add(ToDatabaseEntry(transaction));
        _context.SaveChanges();
    }

    public void WriteTransactions(IEnumerable<Transaction> transactions)
    {
        _context.Transactions.AddRange(transactions.Select(ToDatabaseEntry));
        _context.SaveChanges();
    }

    public void UpdatePrimairyLabel(Transaction transaction, string newPrimairyLabel) => UpdatePrimairyLabel(transaction with { PrimairyLabel = newPrimairyLabel });
    public void UpdatePrimairyLabel(Transaction transaction)
    {
        if (transaction.PrimairyLabel is null) return;

        var matches = _context.Transactions.Where(t => t.RawData == transaction.RawData);

        if (!matches.Any())
        {
            WriteTransaction(transaction);
            return;
        }
        Debug.Assert(matches.Count() == 1);

        var match = matches.First()!;
        match.PrimairyLabel = transaction.PrimairyLabel;
        _context.Transactions.Update(match);
        _context.SaveChanges();
    }

    public void UpdateLabels(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    private TransacionDatabaseEntry ToDatabaseEntry(Transaction transaction)
    {
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
    }

    private Transaction FromDatabaseEntry(TransacionDatabaseEntry transaction)
    {
        return new Transaction
        {
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            ThisPartyIdentifier = transaction.ThisPartyIdentifier,
            ThisPartyName = transaction.ThisPartyName,
            OtherPartyIdentifier = transaction.OtherPartyIdentifier,
            OtherPartyName = transaction.OtherPartyName,
            Timestamp = transaction.Timestamp,
            PaymentReference = NullIfEmptyString(transaction.PaymentReference),
            BIC = NullIfEmptyString(transaction.BIC),
            Description = NullIfEmptyString(transaction.Description),
            RawData = transaction.RawData,
            PrimairyLabel = NullIfEmptyString(transaction.PrimairyLabel),
            Labels = Array.Empty<string>(),
        };

        string? NullIfEmptyString(string value) => value == string.Empty ? null : value;
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
    public DbSet<TransacionDatabaseEntry> Transactions { get; set; }
}


internal sealed class TransacionDatabaseEntry
{
    public Guid Id { get; set; } = new();
    public required long Amount { get; set; }
    public required Currency Currency { get; set; }
    public required string ThisPartyIdentifier { get; set; }
    public required string ThisPartyName { get; set; }
    public required string OtherPartyIdentifier { get; set; }
    public required string OtherPartyName { get; set; }
    public required DateTime Timestamp { get; set; }
    public required string PaymentReference { get; set; }
    public required string BIC { get; set; }
    public required string Description { get; set; }
    public required string RawData { get; set; }
    public required string PrimairyLabel { get; set; }
}
