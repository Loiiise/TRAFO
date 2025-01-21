using Shouldly;
using TRAFO.Logic;
using TRAFO.Logic.Extensions;

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

    [Theory, MemberData(nameof(GenerateLegalTransactionObjects))]
    public void AmountShouldAlwaysBeANumber(Transaction transaction)
    {
        var parser = new MockCSVParser();

        // Get legal values for all other fields
        string
            legalCurrency = transaction.Currency.ToString(),
            legalOtherPartyName = transaction.OtherPartyName,
            legalTimestamp = transaction.Timestamp.ToString();

        // If amount is a Int32 string, it all goes well
        foreach (var legalNumberAmount in new[] { "1", "69", "987654321", "-123456789", "420", "0", "-45781", Int32.MinValue.ToString(), Int32.MaxValue.ToString() })
        {
            var rawData = GenerateRawDataLine(legalNumberAmount, legalCurrency, legalOtherPartyName, legalTimestamp);
            Should.NotThrow(() => parser.Parse(rawData));
        }

        // If amount is a number outside of the Int32 range, the parser throws
        foreach (var outOfRangeNumberAmount in new[] { ((long)Int32.MaxValue + 1).ToString(), ((long)Int32.MinValue - 1).ToString(), "-99999999999", "99999999999" })
        {
            var rawData = GenerateRawDataLine(outOfRangeNumberAmount, legalCurrency, legalOtherPartyName, legalTimestamp);
            var exception = Should.Throw<ArgumentException>(() => parser.Parse(rawData));
            exception.Message.ShouldContain(outOfRangeNumberAmount.ToString());
        }

        // If amount is a not a number at all, the parser throws
        foreach (var notANumberAmount in new[] { "one", "notARealNumber", "John Doe", "throw new ArgumentException()" })
        {
            var rawData = GenerateRawDataLine(notANumberAmount, legalCurrency, legalOtherPartyName, legalTimestamp);
            var exception = Should.Throw<ArgumentException>(() => parser.Parse(rawData));
            exception.Message.ShouldContain(notANumberAmount);
        }
    }

    [Theory, MemberData(nameof(GenerateLegalTransactionObjects))]
    public void CurrencyShouldAlwaysBeACurrency(Transaction transaction)
    {
        var parser = new MockCSVParser();

        // Get legal values for all other fields
        string
            legalAmount = transaction.Amount.ToString(),
            legalOtherPartyName = transaction.OtherPartyName,
            legalTimestamp = transaction.Timestamp.ToString();

        // If currency is a supported currency string, it all goes well
        foreach (string legalCurrencyString in EnumExtensions.GetAllValues<Currency>().Select(currency => currency.ToString()))
        {
            var rawData = GenerateRawDataLine(legalAmount, legalCurrencyString, legalOtherPartyName, legalTimestamp);
            Should.NotThrow(() => parser.Parse(rawData));
        }

        // If currency is a not a currency at all, the parser throws
        foreach (var notACurrencyString in new[] { "NoCurrency", "No money, no problems", "EURALALALA", "I need a dollar" })
        {
            var rawData = GenerateRawDataLine(legalAmount, notACurrencyString, legalOtherPartyName, legalTimestamp);
            Should.Throw<ArgumentException>(() => parser.Parse(rawData));
        }
    }

    [Theory, MemberData(nameof(GenerateLegalTransactionObjects))]
    public void TimestampShouldAlwaysBeADateTime(Transaction transaction)
    {
        var parser = new MockCSVParser();

        // Get legal values for all other fields
        string
            legalAmount = transaction.Amount.ToString(),
            legalCurrency = transaction.Currency.ToString(),
            legalOtherPartyName = transaction.OtherPartyName;

        // If timestamp is a DateTime string, it all goes well
        foreach (var legalDateTimeString in new[] { "2025-01-15T15:15:12", "2024-10-06T10:30:45", "2024-11-12T01:02:03" })
        {
            var rawData = GenerateRawDataLine(legalAmount, legalCurrency, legalOtherPartyName, legalDateTimeString);
            Should.NotThrow(() => parser.Parse(rawData));
        }

        // If amount is a not a number at all, the parser throws
        foreach (var notADateTimeString in new[] { "new DateTime()", "Also not a date time string", "John Doe", "throw new ArgumentException()" })
        {
            var rawData = GenerateRawDataLine(legalAmount, legalCurrency, legalOtherPartyName, notADateTimeString);
            Should.Throw<ArgumentException>(() => parser.Parse(rawData));
        }
    }

    public static IEnumerable<object[]> GenerateLegalTransactionObjects()
        => GenerateLegalTransactions().Select(transaction => new object[] { transaction });
    public static IEnumerable<object[]> GenerateLegalStringAndTransactionObjects()
        => GenerateLegalTransactions().Select(transaction => new object[] { transaction.RawData, transaction });
    private static IEnumerable<Transaction> GenerateLegalTransactions()
    {
        foreach (var amount in new int[] { 23, 12, -504, 1028 })
            foreach (var currency in EnumExtensions.GetAllValues<Currency>())
                foreach (string otherPartyName in new[] { "OTHER PARTY", "John Doe", "Jack Sparrow" })
                    foreach (var timestamp in new[] { new DateTime(2025, 01, 20, 18, 39, 12), new DateTime(2022, 12, 26, 06, 52, 37) })
                        foreach (var category in EnumExtensions.GetAllValues<Category>())
                            yield return (new Transaction
                            {
                                Amount = amount,
                                Currency = currency,
                                OtherPartyName = otherPartyName,
                                Timestamp = timestamp,
                                RawData = GenerateRawDataLine(amount.ToString(), currency.ToString(), otherPartyName, timestamp.ToString()),
                                Category = category,
                            });
    }

    private static string GenerateRawDataLine(string amount, string currency, string otherPartyName, string timestamp, string separator = MockCSVParser.DefaultSeparator)
        => $"{amount}{separator}{currency}{separator}{otherPartyName}{separator}{timestamp}";
}
