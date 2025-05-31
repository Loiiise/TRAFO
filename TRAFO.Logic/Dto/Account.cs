namespace TRAFO.Logic.Dto;

public sealed record Account
{
    public required string AccountId { get; init; }
    public required Balance Balance { get; init; }
    public string? AccountName { get; init; }
}
