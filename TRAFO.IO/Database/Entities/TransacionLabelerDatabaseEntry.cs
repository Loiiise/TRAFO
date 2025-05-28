using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRAFO.IO.Database.Entities;

[PrimaryKey(nameof(TransactionId) + nameof(LabelId))]
internal sealed class TransacionLabelerDatabaseEntry
{
    [ForeignKey(nameof(TransacionDatabaseEntry.TransactionId))]
    public required Guid TransactionId { get; set; }

    [ForeignKey(nameof(LabelDatabaseEntry.LabelId))]
    public required Guid LabelId { get; set; }
}
