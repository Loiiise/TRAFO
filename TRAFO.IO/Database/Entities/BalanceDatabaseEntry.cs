using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRAFO.IO.Database.Entities;

[PrimaryKey(nameof(BalanceId))]
internal sealed class BalanceDatabaseEntry
{
    public required Guid BalanceId { get; set; } = new();
    public required long Amount { get; set; }

    [ForeignKey(nameof(AccountDatabaseEntry.AccountId))]
    public required string ThisPartyAccountId { get; set; }
    public required DateTime Timestamp { get; set; }
    public required bool IsDerived { get; set; }
}
