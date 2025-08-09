using Microsoft.EntityFrameworkCore;
using TRAFO.Logic.Dto;
using TRAFO.Repositories.Entities;

namespace TRAFO.Repositories;

public class EntityFrameworkDatabase
{
    public EntityFrameworkDatabase() : this(new DbContextOptions<EntityFrameworkDatabaseContext>()) { }
    public EntityFrameworkDatabase(DbContextOptions databaseContextOptions)
    {
        _context = new EntityFrameworkDatabaseContext(databaseContextOptions);
    }

    internal BalanceDatabaseEntry ToDatabaseEntry(Balance balance)
    {
        // todo #85
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

    internal static TransacionDatabaseEntry ToDatabaseEntry(Transaction transaction)
    {
        // todo #88
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
    // todo #86
    internal static Label FromDatabaseEntry(LabelDatabaseEntry label) => throw new NotImplementedException();
    internal static Balance FromDatabaseEntry(BalanceDatabaseEntry balance)
    {
        // todo #85
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

    internal static LabelCategory FromDatabaseEntry(LabelCategoryDatabaseEntry labelCategory)
    {
        // todo #87
        throw new NotImplementedException();
    }

    internal static Account FromDatabaseEntry(AccountBalanceDatabaseEntry account)
    {
        // todo #84
        throw new NotImplementedException();
    }

    internal static Transaction FromDatabaseEntry(TransacionDatabaseEntry transaction)
    {
        // todo #88
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

    internal EntityFrameworkDatabaseContext _context;
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
    public DbSet<AccountDatabaseEntry> Account { get; set; }
    public DbSet<AccountBalanceDatabaseEntry> AccountBalance { get; set; }
    public DbSet<LabelBalanceDatabaseEntry> LabelBalance { get; set; }
    public DbSet<LabelCategoryBalanceDatabaseEntry> LabelCategoryBalance { get; set; }
    public DbSet<LabelDatabaseEntry> Label { get; set; }
    public DbSet<TransactionDatabaseEntry> Transaction { get; set; }
    public DbSet<LabelCategoryDatabaseEntry> LabelCategorie { get; set; }
}
