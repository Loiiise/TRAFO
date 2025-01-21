namespace TRAFO.Logic;

public sealed record Transaction
{
    /// <summary>
    /// In cents
    /// </summary>
    public required int Amount { get; init; }
    public required Currency Currency { get; init; }
    public required string OtherPartyName { get; init; }
    public required DateTime Timestamp { get; init; }
    public string PrimairyLabel
    {
        get => _primairyLabel ?? (Labels.Any() ? Labels.First() : string.Empty);
        set
        {
            _primairyLabel = value;
        }
    }
    public required string[] Labels { get; init; }
    public required string RawData { get; init; }

    private string? _primairyLabel;
}