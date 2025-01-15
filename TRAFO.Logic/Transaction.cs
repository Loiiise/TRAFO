namespace TRAFO.Logic;

public sealed record Transaction
{
    public int Amount {  get; init; }
    public required string OtherPartyName { get; init; }
    public DateTime Timestamp { get; init; }
    public required string RawData { get; init; }
    public required string Category { get; init; }
}
