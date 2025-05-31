namespace TRAFO.Logic.Dto;

public sealed record LabelCategory
{
    public required Guid LabelCategoryId { get; init; }
    public required string Name { get; init; }
    public required Balance Balance { get; init; }
    public string? Description { get; init; }
}
