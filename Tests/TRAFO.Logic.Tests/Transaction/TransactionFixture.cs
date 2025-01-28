namespace TRAFO.Logic.Tests;
public static class TransactionFixture
{
    public static Transaction GetEmptyTransaction() => new Transaction
    {
        Amount = 0,
        Currency = Currency.EUR,
        ThisPartyIdentifier = string.Empty,
        OtherPartyIdentifier = string.Empty,
        Timestamp = new DateTime(1970, 01, 01),
        RawData = string.Empty,
        Description = string.Empty,
        Labels = Array.Empty<string>(),
    };
}
