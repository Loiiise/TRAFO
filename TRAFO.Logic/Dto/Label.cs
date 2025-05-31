namespace TRAFO.Logic.Dto;

public sealed record Label
{
    public required Guid LabelId { get; init; }
    public required string Name { get; init; }
    public required Balance Balance { get; init; }
    public required Label[] Children { get; init; }
    public string? Description { get; init; }
    public int? DisplayId { get; init; }
}
