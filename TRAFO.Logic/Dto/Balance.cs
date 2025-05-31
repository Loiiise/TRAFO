namespace TRAFO.Logic.Dto;

public sealed record Balance
{
    public Guid BalanceId { get; init; } = Guid.Empty;
    /// <summary>
    /// In cents
    /// </summary>
    public required long Amount { get; init; }
    public required Currency Currency { get; init; }
    public required DateTime Timestamp { get; init; }
}
