using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRAFO.Repositories.Database.Entities;

[PrimaryKey(nameof(BalanceId))]
internal abstract class BalanceDatabaseEntry
{
    public required Guid BalanceId { get; set; } = new();
    public required long Amount { get; set; }
    public required DateTime Timestamp { get; set; }
    public required bool IsDerived { get; set; }
}

internal sealed class AccountBalanceDatabaseEntry : BalanceDatabaseEntry
{
    [ForeignKey(nameof(AccountDatabaseEntry.AccountId))]
    public required string AccountId { get; set; }
}

internal sealed class LabelBalanceDatabaseEntry : BalanceDatabaseEntry
{
    [ForeignKey(nameof(LabelDatabaseEntry.LabelId))]
    public required Guid LabelId { get; set; }
}

internal sealed class LabelCategoryBalanceDatabaseEntry : BalanceDatabaseEntry
{
    [ForeignKey(nameof(LabelCategoryDatabaseEntry.LabelCategoryId))]
    public required Guid LabelCategoryId { get; set; }
}
