using Shouldly;
using TRAFO.Logic;

namespace TRAFO.Parsing.Tests;

public class CSVParserTests
{
    [Theory, MemberData(nameof(GenerateLegalStringAndTransactionObjects))]
    public void CanParseStandardTransactionLines(string transactionLine, Transaction transaction)
    {
        var parser = new MockCSVParser();

        var result = parser.Parse(transactionLine);

        result.ShouldNotBeNull();        
        result.Amount.ShouldBe(transaction.Amount);
        result.Currency.ShouldBe(transaction.Currency);
        result.OtherPartyName.ShouldBe(transaction.OtherPartyName);
        result.Timestamp.ShouldBe(transaction.Timestamp);
        result.RawData.ShouldBe(transaction.RawData);
        // Category is not set in the parser, so it will always be undefined here
        result.Category.ShouldBe(Category.Undefined);

        parser.Parse(transactionLine).ShouldBe(transaction with { Category = Category.Undefined });
    }

    [Theory, MemberData(nameof(GenerateLegalStringAndTransactionObjects))]
    public void AllIndicesShouldBeInRange(string transactionLine, Transaction transaction)
    {
        // Default parser, only legal values
        var parser = new MockCSVParser(0, 1, 2, 3);
        Should.NotThrow(() => parser.Parse(transactionLine));
        parser.Parse(transactionLine).ShouldBe(transaction with { Category = Category.Undefined });

        // Any index out of range should throw
        foreach (var throwingParser in new[]
            {
                new MockCSVParser(99, 1, 2, 3),
                new MockCSVParser(0, 99, 2, 3),
                new MockCSVParser(0, 1, 99, 3),
                new MockCSVParser(0, 1, 2, 99),
                new MockCSVParser(99,99,99,99),
            })
        {
            Should.Throw<IndexOutOfRangeException>(() => throwingParser.Parse(transactionLine));
        }
    }

    public static IEnumerable<object[]> GenerateLegalStringAndTransactionObjects()
        => GenerateLegalStringsAndTransactions().Select(st => new object[] { st.Item1, st.Item2 });
    private static IEnumerable<(string, Transaction)> GenerateLegalStringsAndTransactions()
    {
        var sep = MockCSVParser.DefaultSeparator;

        foreach (var amount in new int[] { 23, 12, -504, 1028 })
            foreach (var currency in Enum.GetValues(typeof(Currency)).Cast<Currency>())
                foreach (string otherParty in new[] { "OTHER PARTY", "John Doe", "Jack Sparrow" })
                    foreach (var timestamp in new[] { new DateTime(2025, 01, 20, 18, 39, 12), new DateTime(2022, 12, 26, 06, 52, 37) })
                        foreach (var category in Enum.GetValues(typeof(Category)).Cast<Category>())
                        {
                            var rawData = $"{amount}{sep}{currency}{sep}{otherParty}{sep}{timestamp}";
                            yield return (rawData, new Transaction
                            {
                                Amount = amount,
                                Currency = currency,
                                OtherPartyName = otherParty,
                                Timestamp = timestamp,
                                RawData = rawData,
                                Category = category,
                            });
                        }
    }
}
