using Microsoft.EntityFrameworkCore;

namespace TRAFO.Repositories.Entities;

[PrimaryKey(nameof(TransactionId))]
internal sealed class TransacionDatabaseEntry
{
    public required Guid TransactionId { get; set; } = new();
    public Guid? ParentTransactionId { get; set; }
    public required long Amount { get; set; }

    public required AccountDatabaseEntry ThisPartyAccount { get; set; }
    public required AccountDatabaseEntry OtherPartyAccount { get; set; }
    public required DateTime Timestamp { get; set; }
    public string? PaymentReference { get; set; }
    public string? BIC { get; set; }
    public string? Description { get; set; }
    public string? RawData { get; set; }

    public required ICollection<TransacionLabelerDatabaseEntry> Labels { get; set; }
}
