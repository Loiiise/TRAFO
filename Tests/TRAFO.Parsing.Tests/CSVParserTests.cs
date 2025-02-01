using Shouldly;
using System.Numerics;
using TRAFO.Logic;
using TRAFO.Logic.Extensions;
using static TRAFO.Logic.Tests.TransactionFixture;

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
        result.ThisPartyIdentifier.ShouldBe(transaction.ThisPartyIdentifier);
        result.OtherPartyIdentifier.ShouldBe(transaction.OtherPartyIdentifier);
        result.Timestamp.ShouldBe(transaction.Timestamp);
        result.RawData.ShouldBe(transaction.RawData);
        // Labels are not set in the parser, so the collection will always be empty
        result.Labels.ShouldBe(Array.Empty<string>());

        parser.Parse(transactionLine).ShouldBe(transaction with { Labels = Array.Empty<string>() });
    }

    [Theory, MemberData(nameof(GenerateLegalStringAndTransactionObjects))]
    public void AllIndicesShouldBeInRange(string transactionLine, Transaction transaction)
    {
        // Default parser, only legal values
        var parser = new MockCSVParser(0, 1, 2, 3, 4);
        Should.NotThrow(() => parser.Parse(transactionLine));

        // Labels are not set in the parser, so the collection will always be empty
        var parserResult = parser.Parse(transactionLine);
        parserResult.ShouldBe(transaction with { Labels = Array.Empty<string>() });

        // Any index out of range should throw
        foreach (var throwingParser in new[]
            {
                new MockCSVParser(99, 1, 2, 3, 4),
                new MockCSVParser( 0,99, 2, 3, 4),
                new MockCSVParser( 0, 1,99, 3, 4),
                new MockCSVParser( 0, 1, 2,99, 4),
                new MockCSVParser( 0, 1, 2, 3,99),
                new MockCSVParser(99,99,99,99,99),
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
            legalThisPartyIdentifier = transaction.ThisPartyIdentifier,
            legalOtherPartyIdentifier = transaction.OtherPartyIdentifier,
            legalTimestamp = transaction.Timestamp.ToString();

        // If amount is a Int32 string, it all goes well
        foreach (var legalNumberAmount in new[] { "1", "69", "987654321", "-123456789", "420", "0", "-45781", Int32.MinValue.ToString(), Int32.MaxValue.ToString() })
        {
            var rawData = GenerateBasicRawDataLine(legalNumberAmount, legalCurrency, legalThisPartyIdentifier, legalOtherPartyIdentifier, legalTimestamp);
            Should.NotThrow(() => parser.Parse(rawData));
        }

        // If amount is a number outside of the Int32 range, the parser throws
        foreach (var outOfRangeNumberAmount in new[] { ((BigInteger)long.MaxValue + 1).ToString(), ((BigInteger)long.MinValue - 1).ToString(), "-99999999999999999999", "99999999999999999999" })
        {
            var rawData = GenerateBasicRawDataLine(outOfRangeNumberAmount, legalCurrency, legalThisPartyIdentifier, legalOtherPartyIdentifier, legalTimestamp);
            var exception = Should.Throw<ArgumentException>(() => parser.Parse(rawData));
            exception.Message.ShouldContain(outOfRangeNumberAmount.ToString());
        }

        // If amount is a not a number at all, the parser throws
        foreach (var notANumberAmount in new[] { "one", "notARealNumber", "John Doe", "throw new ArgumentException()" })
        {
            var rawData = GenerateBasicRawDataLine(notANumberAmount, legalCurrency, legalThisPartyIdentifier, legalOtherPartyIdentifier, legalTimestamp);
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
            legalThisPartyIdentifier = transaction.ThisPartyIdentifier,
            legalOtherPartyIdentifier = transaction.OtherPartyIdentifier,
            legalTimestamp = transaction.Timestamp.ToString();

        // If currency is a supported currency string, it all goes well
        foreach (string legalCurrencyString in EnumExtensions.GetAllValues<Currency>().Select(currency => currency.ToString()))
        {
            var rawData = GenerateBasicRawDataLine(legalAmount, legalCurrencyString, legalThisPartyIdentifier, legalOtherPartyIdentifier, legalTimestamp);
            Should.NotThrow(() => parser.Parse(rawData));
        }

        // If currency is a not a currency at all, the parser throws
        foreach (var notACurrencyString in new[] { "NoCurrency", "No money, no problems", "EURALALALA", "I need a dollar" })
        {
            var rawData = GenerateBasicRawDataLine(legalAmount, notACurrencyString, legalThisPartyIdentifier, legalOtherPartyIdentifier, legalTimestamp);
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
            legalThisPartyIdentifier = transaction.ThisPartyIdentifier,
            legalOtherPartyIdentifier = transaction.OtherPartyIdentifier;

        // If timestamp is a DateTime string, it all goes well
        foreach (var legalDateTimeString in new[] { "2025-01-15T15:15:12", "2024-10-06T10:30:45", "2024-11-12T01:02:03" })
        {
            var rawData = GenerateBasicRawDataLine(legalAmount, legalCurrency, legalThisPartyIdentifier, legalOtherPartyIdentifier, legalDateTimeString);
            Should.NotThrow(() => parser.Parse(rawData));
        }

        // If amount is a not a number at all, the parser throws
        foreach (var notADateTimeString in new[] { "new DateTime()", "Also not a date time string", "John Doe", "throw new ArgumentException()" })
        {
            var rawData = GenerateBasicRawDataLine(legalAmount, legalCurrency, legalThisPartyIdentifier, legalOtherPartyIdentifier, notADateTimeString);
            Should.Throw<ArgumentException>(() => parser.Parse(rawData));
        }
    }

    [Fact]
    public void TestNewTransactionLogic()
    {
        throw new NotImplementedException();
    }

    [Theory, CombinatorialData]
    public void TransactionWitThisPartyNameCanBeParsed(
        [CombinatorialMemberData(nameof(GenerateLegalTransactionObjects))] Transaction transaction,
        [CombinatorialValues("it is I", "we da party")] string thisPartyName)
        => ParseRawDataWithOptionalFieldTestHelper(
            MockCSVParser.GetBasicCSVParserWithThisPartyNameIndex(),
            transaction,
            thisPartyName,
            parseResult => parseResult.ThisPartyName.ShouldBe(thisPartyName));

    [Theory, CombinatorialData]
    public void TransactionWithOtherPartyNameCanBeParsed(
        [CombinatorialMemberData(nameof(GenerateLegalTransactionObjects))] Transaction transaction,
        [CombinatorialValues("opponents", "the others", "they/them")] string otherPartyName)
        => ParseRawDataWithOptionalFieldTestHelper(
            MockCSVParser.GetBasicCSVParserWithOtherPartyNameIndex(),
            transaction,
            otherPartyName,
            parseResult => parseResult.OtherPartyName.ShouldBe(otherPartyName));

    [Theory, CombinatorialData]
    public void TransactionWithPaymentReferenceCanBeParsed(
        [CombinatorialMemberData(nameof(GenerateLegalTransactionObjects))] Transaction transaction,
        [CombinatorialValues("Invoice 202500000001", "Order 69420")] string paymentReference)
        => ParseRawDataWithOptionalFieldTestHelper(
            MockCSVParser.GetBasicCSVParserWithPaymentReferenceIndex(),
            transaction,
            paymentReference,
            parseResult => parseResult.PaymentReference.ShouldBe(paymentReference));

    [Theory, CombinatorialData]
    public void TransactionWithBICCanBeParsed(
        [CombinatorialMemberData(nameof(GenerateLegalTransactionObjects))] Transaction transaction,
        [CombinatorialValues("HBMBNL69", "ABCDCC00111")] string bic)
        => ParseRawDataWithOptionalFieldTestHelper(
            MockCSVParser.GetBasicCSVParserWithBICIndex(),
            transaction,
            bic,
            parseResult => parseResult.BIC.ShouldBe(bic));

    [Theory, CombinatorialData]
    public void TransactionWithDescriptionCanBeParsed(
        [CombinatorialMemberData(nameof(GenerateLegalTransactionObjects))] Transaction transaction,
        [CombinatorialValues("blablabla", "I describe", "The Boons and the Banes")] string description)
        => ParseRawDataWithOptionalFieldTestHelper(
            MockCSVParser.GetBasicCSVParserWithDescriptionIndex(),
            transaction,
            description,
            parseResult => parseResult.Description.ShouldBe(description));

    private void ParseRawDataWithOptionalFieldTestHelper(MockCSVParser parser, Transaction transaction, string optionalValue, Action<Transaction> checkParseResult)
    {
        // Get legal values for all other fields
        string
            legalAmount = transaction.Amount.ToString(),
            legalCurrency = transaction.Currency.ToString(),
            legalThisPartyIdentifier = transaction.ThisPartyIdentifier,
            legalOtherPartyIdentifier = transaction.OtherPartyIdentifier,
            legalTimestamp = transaction.Timestamp.ToString();

        var rawData = GenerateBasicRawDataLineWithOptionalField(legalAmount, legalCurrency, legalThisPartyIdentifier, legalOtherPartyIdentifier, legalTimestamp, optionalValue);

        var parseResult = parser.Parse(rawData);
        checkParseResult(parseResult);
    }

    [Fact]
    public void OtherSeparatoriueshtiuesh()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<object[]> GenerateLegalTransactionObjects()
        => GenerateLegalTransactionsWithCSVRawData().Select(transaction => new object[] { transaction });
    public static IEnumerable<object[]> GenerateLegalStringAndTransactionObjects()
        => GenerateLegalTransactionsWithCSVRawData().Select(transaction => new object[] { transaction.RawData, transaction });

    private static IEnumerable<Transaction> GenerateLegalTransactionsWithCSVRawData()
        => GenerateBasicLegalTransactions((amount, currency, thisPartyIdentifier, otherPartyIdentifier, timestamp) => GenerateBasicRawDataLine(amount.ToString(), currency.ToString(), thisPartyIdentifier, otherPartyIdentifier, timestamp.ToString()));

    private static string GenerateBasicRawDataLine(string amount, string currency, string thisPartyIdentifier, string otherPartyIdentifier, string timestamp, string separator = MockCSVParser.DefaultSeparator)
        => $"{amount}{separator}{currency}{separator}{thisPartyIdentifier}{separator}{otherPartyIdentifier}{separator}{timestamp}";
    private static string GenerateBasicRawDataLineWithOptionalField(string amount, string currency, string thisPartyIdentifier, string otherPartyIdentifier, string timestamp, string optionalFieldValue, string separator = MockCSVParser.DefaultSeparator)
        => GenerateBasicRawDataLine(amount, currency, thisPartyIdentifier, otherPartyIdentifier, timestamp, separator) + $"{separator}{optionalFieldValue}";
}