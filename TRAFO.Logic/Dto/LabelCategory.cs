namespace TRAFO.Logic.Dto;

public sealed record LabelCategory
{
    public Guid LabelCategoryId { get; init; } = Guid.Empty;
    public required string Name { get; init; }
    public required Balance Balance { get; init; }
    public string? Description { get; init; }
}
