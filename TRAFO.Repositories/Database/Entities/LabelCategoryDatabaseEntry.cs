using Microsoft.EntityFrameworkCore;

namespace TRAFO.Repositories.Database.Entities;

[PrimaryKey(nameof(LabelCategoryId))]
internal sealed class LabelCategoryDatabaseEntry
{
    public required Guid LabelCategoryId { get; set; } = new();
    public required string Name { get; set; }
    public string? Description { get; set; }
}
