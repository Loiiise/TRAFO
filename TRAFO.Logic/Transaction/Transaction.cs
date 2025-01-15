namespace TRAFO.Logic;

public sealed record Transaction
{
    public required int Amount { get; init; }
    public required Currency Currency { get; init; }
    public required string OtherPartyName { get; init; }
    public required DateTime Timestamp { get; init; }
    public required string RawData { get; init; }
    public required Category Category { get; init; }
}
