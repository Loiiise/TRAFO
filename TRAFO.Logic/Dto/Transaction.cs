namespace TRAFO.Logic.Dto;

public sealed record Transaction
{
    public Guid TransactionId { get; init; } = Guid.Empty;

    /// <summary>
    /// In cents
    /// </summary>
    public required long Amount { get; init; }

    public required Currency Currency { get; init; }

    public required string ThisAccountIdentifier { get; init; }
    public string ThisAccountName
    {
        get => _thisPartyName ?? ThisAccountIdentifier;
        init => _thisPartyName = value;
    }

    public required string OtherAccountIdentifier { get; init; }
    public string OtherPartyName
    {
        get => _otherPartyName ?? OtherAccountIdentifier;
        init => _otherPartyName = value;
    }

    public required DateTime Timestamp { get; init; }

    public string? PaymentReference { get; init; }
    public string? BIC { get; init; }
    public string? Description { get; init; }

    public required string RawData { get; init; }

    public required string[] Labels { get; init; }

    private readonly string? _thisPartyName;
    private readonly string? _otherPartyName;

    public override string ToString() => $"Transaction of {ShowAmount()} {Currency} on {Timestamp} from {ThisAccountIdentifier} to {OtherAccountIdentifier} with description {Description}";

    private string ShowAmount() => ShowAmount(Amount, Currency);
    public static string ShowAmount(long amount, Currency currency)
        => currency is Currency.EUR
            ? $"{amount / 100},{GetEuroCentString(amount % 100)}"
            : amount.ToString();

    private static string GetEuroCentString(long cents)
        => cents == 0
        ? "00"
        : Math.Abs(cents).ToString();
}