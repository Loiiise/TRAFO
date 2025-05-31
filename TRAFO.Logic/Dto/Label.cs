namespace TRAFO.Logic.Dto;

public sealed record Label
{
    public Guid LabelId { get; init; } = Guid.Empty;
    public required string Name { get; init; }
    public Balance? Balance { get; init; }
    public Label[]? Children { get; init; }
    public string? Description { get; init; }
    public int? DisplayId { get; init; }
}
