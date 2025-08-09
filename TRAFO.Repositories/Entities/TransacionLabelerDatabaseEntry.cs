using Microsoft.EntityFrameworkCore;

namespace TRAFO.Repositories.Entities;

[PrimaryKey(nameof(Transaction.TransactionId) + nameof(Label.LabelId))]
internal sealed class TransacionLabelerDatabaseEntry
{
    public required TransacionDatabaseEntry Transaction { get; set; }

    public required LabelDatabaseEntry Label { get; set; }
}
