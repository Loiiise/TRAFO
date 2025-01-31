using TRAFO.Logic.Extensions;

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

    public static IEnumerable<Transaction> GenerateBasicLegalTransactionsWithoutRawData()
        => GenerateBasicLegalTransactions(((_, _, _, _, _) => string.Empty));

    public static IEnumerable<Transaction> GenerateBasicLegalTransactions(Func<long, Currency, string, string, DateTime, string> generateRawDataLine)
    {
        foreach (var amount in new long[] { 23, 12, -504, 1028 })
            foreach (var currency in EnumExtensions.GetAllValues<Currency>())
                foreach (string thisPartyIdentifier in new[] { "THIS PARTY", "me", "myself", "I" })
                    foreach (string otherPartyIdentifier in new[] { "OTHER PARTY", "John Doe", "Jack Sparrow" })
                        foreach (var timestamp in new[] { new DateTime(2025, 01, 20, 18, 39, 12), new DateTime(2022, 12, 26, 06, 52, 37) })
                            foreach (var labels in new[] { Array.Empty<string>(), new[] { "label0", "i dont wanna be a label" } })
                                yield return new Transaction
                                {
                                    Amount = amount,
                                    Currency = currency,
                                    ThisPartyIdentifier = otherPartyIdentifier,
                                    OtherPartyIdentifier = otherPartyIdentifier,
                                    Timestamp = timestamp,
                                    RawData = generateRawDataLine(amount, currency, thisPartyIdentifier, otherPartyIdentifier, timestamp),
                                    Labels = labels,
                                };
    }
}
