using Microsoft.EntityFrameworkCore;
using TRAFO.Logic;

namespace TRAFO.IO.Database;

public class EntityFrameworkDatabase : IDatabase
{
    public EntityFrameworkDatabase() : this(new DbContextOptions<EntityFrameworkDatabaseContext>()) { }
    private EntityFrameworkDatabase(DbContextOptions databaseContextOptions)
    {
        _context = new EntityFrameworkDatabaseContext(databaseContextOptions);
    }

    public IEnumerable<Transaction> ReadAllTransactions()
    {
        return _context.Transactions.Select(FromDatabaseEntry);
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
            Description = transaction.Description,
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
            Description = transaction.Description,
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
    public EntityFrameworkDatabaseContext() : this (new DbContextOptions<EntityFrameworkDatabaseContext>()) { }
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
    public required long Amount { get; init; }
    public required Currency Currency { get; init; }
    public required string ThisPartyIdentifier { get; init; }
    public required string ThisPartyName { get; init; }
    public required string OtherPartyIdentifier { get; init; }
    public required string OtherPartyName { get; init; }
    public required DateTime Timestamp { get; init; }
    public required string PaymentReference { get; init; }
    public required string BIC { get; init; }
    public required string Description { get; init; }
    public required string RawData { get; init; }
    public required string PrimairyLabel { get; init; }
}
