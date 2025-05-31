namespace TRAFO.Logic.Dto;

public sealed record Balance
{
    public required Guid BalanceId { get; init; }
    /// <summary>
    /// In cents
    /// </summary>
    public required long Amount { get; init; }
    public required Currency Currency { get; init; }
    public required DateTime Timestamp { get; init; }
}
