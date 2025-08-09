using Microsoft.EntityFrameworkCore;
using TRAFO.Repositories.Entities;

namespace TRAFO.Repositories;

public class EntityFrameworkDatabaseContext : DbContext
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
    internal DbSet<AccountDatabaseEntry> Account { get; set; }
    internal DbSet<AccountBalanceDatabaseEntry> AccountBalance { get; set; }
    internal DbSet<LabelBalanceDatabaseEntry> LabelBalance { get; set; }
    internal DbSet<LabelCategoryBalanceDatabaseEntry> LabelCategoryBalance { get; set; }
    internal DbSet<LabelDatabaseEntry> Label { get; set; }
    internal DbSet<LabelCategoryDatabaseEntry> LabelCategory { get; set; }
    internal DbSet<TransactionDatabaseEntry> Transaction { get; set; }
    internal DbSet<TransacionLabelerDatabaseEntry> TransactionLabels { get; set; }
}
