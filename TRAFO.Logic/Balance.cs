namespace TRAFO.Logic;

public sealed record Balance
{
    /// <summary>
    /// In cents
    /// </summary>
    public required long Amount { get; init; }
    public required Currency Currency { get; init; }
    public required string ThisPartyIdentifier { get; init; }
    public required DateTime Timestamp { get; init; }
}
