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

    public string Description 
    { 
        get => _description ?? $"Transaction of {Amount} {Currency} on {Timestamp} from {ThisPartyIdentifier} to {OtherPartyIdentifier}";
        init => _description = value;
    }
    
    public required string RawData { get; init; }

    public string? PrimairyLabel
    {
        get => throw new NotImplementedException();
        init => throw new NotImplementedException();        
    }

    public required string[] Labels 
    { 
        get => throw new NotImplementedException();
        init => throw new NotImplementedException();
    }

    private readonly string? _thisPartyName;
    private readonly string? _otherPartyName;

    private readonly string? _description;
}