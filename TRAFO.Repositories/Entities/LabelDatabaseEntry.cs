using Microsoft.EntityFrameworkCore;

namespace TRAFO.Repositories.Entities;

[PrimaryKey(nameof(LabelId))]
internal sealed class LabelDatabaseEntry
{
    public required Guid LabelId { get; set; } = new();
    public Guid? ParentLabelId { get; set; }
    public required string Name { get; set; }

    public required LabelCategoryDatabaseEntry LabelCategory { get; set; }
    public string? Description { get; set; }
    public int? DisplayId { get; set; }

    public required ICollection<LabelBalanceDatabaseEntry> Balances { get; set; }
    public required ICollection<TransacionLabelerDatabaseEntry> Transactions { get; set; }
}
