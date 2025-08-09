using Microsoft.EntityFrameworkCore;
using TRAFO.Repositories.Entities;

namespace TRAFO.Repositories;

public class EntityFrameworkDatabase
{
    public EntityFrameworkDatabase() : this(new DbContextOptions<EntityFrameworkDatabaseContext>()) { }
    public EntityFrameworkDatabase(DbContextOptions databaseContextOptions)
    {
        throw new NotSupportedException();
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
    public DbSet<LabelCategoryDatabaseEntry> LabelCategory { get; set; }
    public DbSet<TransactionDatabaseEntry> Transaction { get; set; }
    public DbSet<TransacionLabelerDatabaseEntry> TransactionLabels { get; set; }
}
