namespace TRAFO.Logic;

public sealed record Transaction
{
    /// <summary>
    /// In cents
    /// </summary>
    public required long Amount { get; init; }

    public required Currency Currency { get; init; }

    public required string OtherPartyName { get; init; }

    public required DateTime Timestamp { get; init; }

    public required string Description { get; init; }
    
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
}