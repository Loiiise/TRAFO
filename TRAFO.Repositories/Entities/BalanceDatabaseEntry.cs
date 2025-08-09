using Microsoft.EntityFrameworkCore;

namespace TRAFO.Repositories.Entities;

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
    public required AccountDatabaseEntry Account { get; set; }
}

internal sealed class LabelBalanceDatabaseEntry : BalanceDatabaseEntry
{
    public required LabelDatabaseEntry Label { get; set; }
}

internal sealed class LabelCategoryBalanceDatabaseEntry : BalanceDatabaseEntry
{
    public required LabelCategoryBalanceDatabaseEntry LabelCategory { get; set; }
}
