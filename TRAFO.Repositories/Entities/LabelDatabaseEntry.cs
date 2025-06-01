using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRAFO.Repositories.Entities;

[PrimaryKey(nameof(LabelId))]
internal sealed class LabelDatabaseEntry
{
    public required Guid LabelId { get; set; } = new();
    public Guid? ParentLabelId { get; set; }
    public required string Name { get; set; }

    [ForeignKey(nameof(LabelCategoryDatabaseEntry.LabelCategoryId))]
    public required Guid LabelCategoryId { get; set; } = new();
    public string? Description { get; set; }
    public int? DisplayId { get; set; }
}
