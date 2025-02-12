namespace TRAFO.Logic;

public sealed record Transaction
{
    /// <summary>
    /// In cents
    /// </summary>
    public required long Amount { get; init; }

    public required Currency Currency { get; init; }

    public required string ThisPartyIdentifier { get; init; }
    public string ThisPartyName
    {
        get => _thisPartyName ?? ThisPartyIdentifier;
        init => _thisPartyName = value;
    }

    public required string OtherPartyIdentifier { get; init; }
    public string OtherPartyName
    {
        get => _otherPartyName ?? OtherPartyIdentifier;
        init => _otherPartyName = value;
    }

    public required DateTime Timestamp { get; init; }

    public string? PaymentReference { get; init; }
    public string? BIC { get; init; }
    public string? Description { get; init; }

    public required string RawData { get; init; }

    public string? PrimairyLabel { get; init; }
    public required string[] Labels { get; init; }

    private readonly string? _thisPartyName;
    private readonly string? _otherPartyName;

    public override string ToString() => $"Transaction of {ShowAmount()} {Currency} on {Timestamp} from {ThisPartyIdentifier} to {OtherPartyIdentifier} with description {Description}";

    private string ShowAmount() => Currency is Currency.EUR
        ? $"{Amount / 100},{GetEuroCentString(Amount % 100)}"
        : Amount.ToString();

    private string GetEuroCentString(long cents) 
        => cents == 0 
        ? "00"
        : Math.Abs(cents).ToString();

}