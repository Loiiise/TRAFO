using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRAFO.Repositories.Entities;

[PrimaryKey(nameof(TransactionId))]
internal sealed class TransacionDatabaseEntry
{
    public required Guid TransactionId { get; set; } = new();
    public Guid? ParentTransactionId { get; set; }
    public required long Amount { get; set; }

    [ForeignKey(nameof(AccountDatabaseEntry.AccountId))]
    public required string ThisPartyAccountId { get; set; }
    public string? ThisPartyName { get; set; }
    public required string OtherPartyAccountId { get; set; }
    public string? OtherPartyName { get; set; }
    public required DateTime Timestamp { get; set; }
    public string? PaymentReference { get; set; }
    public string? BIC { get; set; }
    public string? Description { get; set; }
    public string? RawData { get; set; }

    public required ICollection<TransacionLabelerDatabaseEntry> Labels { get; set; }
}
