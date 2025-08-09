using TRAFO.Logic.Extensions;

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
        get => _thisAccountName ?? ThisAccountIdentifier;
        init => _thisAccountName = value;
    }

    public required string OtherAccountIdentifier { get; init; }
    public string OtherAccountName
    {
        get => _otherAccountName ?? OtherAccountIdentifier;
        init => _otherAccountName = value;
    }

    public required DateTime Timestamp { get; init; }

    public string? PaymentReference { get; init; }
    public string? BIC { get; init; }
    public string? Description { get; init; }

    public required string RawData { get; init; }

    public required string[] Labels { get; init; }

    private readonly string? _thisAccountName;
    private readonly string? _otherAccountName;

    public override string ToString() => $"Transaction of {this.ShowAmount()} {Currency} on {Timestamp} from {ThisAccountIdentifier} to {OtherAccountIdentifier} with description {Description}";    
}