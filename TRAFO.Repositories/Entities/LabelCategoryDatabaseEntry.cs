using Microsoft.EntityFrameworkCore;

namespace TRAFO.Repositories.Entities;

[PrimaryKey(nameof(LabelCategoryId))]
internal sealed class LabelCategoryDatabaseEntry
{
    public required Guid LabelCategoryId { get; set; } = new();
    public required string Name { get; set; }
    public string? Description { get; set; }

    public required ICollection<LabelCategoryBalanceDatabaseEntry> Balances { get; set; }
    public required ICollection<LabelDatabaseEntry> Labels { get; set; }
}
