using Microsoft.EntityFrameworkCore;

namespace TRAFO.Repositories.Entities;

[PrimaryKey(nameof(TransactionId))]
internal sealed class TransactionDatabaseEntry
{
    public required Guid TransactionId { get; set; } = new();
    public TransactionDatabaseEntry? ParentTransaction { get; set; }
    public required long Amount { get; set; }

    public required AccountDatabaseEntry ThisPartyAccount { get; set; }
    public required AccountDatabaseEntry OtherPartyAccount { get; set; }
    public required DateTime Timestamp { get; set; }
    public string? PaymentReference { get; set; }
    public string? BIC { get; set; }
    public string? Description { get; set; }
    public string? RawData { get; set; }
    
    public ICollection<TransacionLabelerDatabaseEntry> Labels { get; set; } = new List<TransacionLabelerDatabaseEntry>();
}
