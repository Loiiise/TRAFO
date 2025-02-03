using Shouldly;
using System.Numerics;
using TRAFO.Logic;
using TRAFO.Logic.Extensions;
using static TRAFO.Logic.Tests.TransactionFixture;

namespace TRAFO.Parsing.Tests;

public class CSVParserTests
{
    [Theory, MemberData(nameof(GenerateLegalRawDataAndTransactions))]
    public void CanParseStandardTransactionLines(string transactionLine, Transaction transaction)
    {
        var parser = new MockCSVParser();

        var parseResult = parser.Parse(transactionLine);

        parseResult.ShouldNotBeNull();
        parseResult.Amount.ShouldBe(transaction.Amount);
        parseResult.Currency.ShouldBe(transaction.Currency);
        parseResult.ThisPartyIdentifier.ShouldBe(transaction.ThisPartyIdentifier);
        parseResult.OtherPartyIdentifier.ShouldBe(transaction.OtherPartyIdentifier);
        parseResult.Timestamp.ShouldBe(transaction.Timestamp);
        parseResult.RawData.ShouldBe(transaction.RawData);
        // Labels are not set in the parser, so the collection will always be empty
        parseResult.Labels.ShouldBe(Array.Empty<string>());

        parser.Parse(transactionLine).ShouldBe(transaction with { Labels = Array.Empty<string>() });
    }

    [Theory, MemberData(nameof(GenerateLegalRawDataAndTransactions))]
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

    [Fact]
    public void MandatoryFieldsCanBeParsedInAnyOrder()
    {
        var transaction = GenerateOneBasicLegalTransactionWithoutRawData();
        int totalAmountOfFiels = 10;

        for (var i = 0; i < totalAmountOfFiels; i++)
            for (var j = 0; j < totalAmountOfFiels; j++)
                for (var k = 0; k < totalAmountOfFiels; k++)
                    for (var l = 0; l < totalAmountOfFiels; l++)
                        for (var m = 0; m < totalAmountOfFiels; m++)
                        {
                            var indices = new[] { i, j, k, l, m };
                            if (indices.Length != indices.Distinct().Count())
                            {
                                // Each index has to be unique, otherwise they'll overwrite eachother.
                                continue;
                            }

                            var mockDataFiels = new string[totalAmountOfFiels];
                            mockDataFiels[i] = transaction.Amount.ToString();
                            mockDataFiels[j] = transaction.Currency.ToString();
                            mockDataFiels[k] = transaction.ThisPartyIdentifier;
                            mockDataFiels[l] = transaction.OtherPartyIdentifier;
                            mockDataFiels[m] = transaction.Timestamp.ToString();

                            var parser = new MockCSVParser(i, j, k, l, m);
                            var rawData = GenerateSeperatedValueString(MockCSVParser.DefaultSeparator, mockDataFiels);

                            var parseResult = parser.Parse(rawData);

                            parseResult.Amount.ShouldBe(transaction.Amount);
                            parseResult.Currency.ShouldBe(transaction.Currency);
                            parseResult.ThisPartyIdentifier.ShouldBe(transaction.ThisPartyIdentifier);
                            parseResult.OtherPartyIdentifier.ShouldBe(transaction.OtherPartyIdentifier);
                            parseResult.Timestamp.ShouldBe(transaction.Timestamp);
                        }
    }

    [Theory, MemberData(nameof(GenerateLegalTransactions))]
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

    [Theory, MemberData(nameof(GenerateLegalTransactions))]
    public void AmountCanBeParsedRegardlessOfSeperatorSignsAndOtherIlligalCharacters(Transaction transaction)
    {
        var parser = new MockCSVParser();

        // Get legal values for all other fields
        string
            legalCurrency = transaction.Currency.ToString(),
            legalThisPartyIdentifier = transaction.ThisPartyIdentifier,
            legalOtherPartyIdentifier = transaction.OtherPartyIdentifier,
            legalTimestamp = transaction.Timestamp.ToString();

        foreach ((string legalNumberAmount, long expectedAmount) in new[]
            {
                ("1,00", 100),
                ("-35,00", -3500),
                ("12.34", 1234),
                ("+3483,34", 348334),
                ("1,0blablablba3", 103),
            })
        {
            var rawData = GenerateBasicRawDataLine(legalNumberAmount, legalCurrency, legalThisPartyIdentifier, legalOtherPartyIdentifier, legalTimestamp);
            
            var parseResult = parser.Parse(rawData);
            parseResult.Amount.ShouldBe(expectedAmount);
        }
    }

    [Theory, MemberData(nameof(GenerateLegalTransactions))]
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
        foreach (string legalCurrencyString in EnumExtensions.CommonCurrencies().Select(currency => currency.ToString()))
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

    [Theory, MemberData(nameof(GenerateLegalTransactions))]
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

    [Theory, CombinatorialData]
    public void TransactionWitThisPartyNameCanBeParsed(
        [CombinatorialMemberData(nameof(GenerateLegalTransactions))] Transaction transaction,
        [CombinatorialMemberData(nameof(GetExamplesFor), nameof(Transaction.ThisPartyName))] string thisPartyName)
        => ParseRawDataWithOptionalFieldTestHelper(
            MockCSVParser.GetBasicCSVParserWithThisPartyNameIndex(),
            transaction,
            thisPartyName,
            parseResult => parseResult.ThisPartyName.ShouldBe(thisPartyName));

    [Theory, CombinatorialData]
    public void TransactionWithOtherPartyNameCanBeParsed(
        [CombinatorialMemberData(nameof(GenerateLegalTransactions))] Transaction transaction,
        [CombinatorialMemberData(nameof(GetExamplesFor), nameof(Transaction.OtherPartyName))] string otherPartyName)
        => ParseRawDataWithOptionalFieldTestHelper(
            MockCSVParser.GetBasicCSVParserWithOtherPartyNameIndex(),
            transaction,
            otherPartyName,
            parseResult => parseResult.OtherPartyName.ShouldBe(otherPartyName));

    [Theory, CombinatorialData]
    public void TransactionWithPaymentReferenceCanBeParsed(
        [CombinatorialMemberData(nameof(GenerateLegalTransactions))] Transaction transaction,
        [CombinatorialMemberData(nameof(GetExamplesFor), nameof(Transaction.PaymentReference))] string paymentReference)
        => ParseRawDataWithOptionalFieldTestHelper(
            MockCSVParser.GetBasicCSVParserWithPaymentReferenceIndex(),
            transaction,
            paymentReference,
            parseResult => parseResult.PaymentReference.ShouldBe(paymentReference));

    [Theory, CombinatorialData]
    public void TransactionWithBICCanBeParsed(
        [CombinatorialMemberData(nameof(GenerateLegalTransactions))] Transaction transaction,
        [CombinatorialMemberData(nameof(GetExamplesFor), nameof(Transaction.BIC))] string bic)
        => ParseRawDataWithOptionalFieldTestHelper(
            MockCSVParser.GetBasicCSVParserWithBICIndex(),
            transaction,
            bic,
            parseResult => parseResult.BIC.ShouldBe(bic));

    [Theory, CombinatorialData]
    public void TransactionWithDescriptionCanBeParsed(
        [CombinatorialMemberData(nameof(GenerateLegalTransactions))] Transaction transaction,
        [CombinatorialMemberData(nameof(GetExamplesFor), nameof(Transaction.Description))] string description)
        => ParseRawDataWithOptionalFieldTestHelper(
            MockCSVParser.GetBasicCSVParserWithDescriptionIndex(),
            transaction,
            description,
            parseResult => parseResult.Description.ShouldBe(description));

    private void ParseRawDataWithOptionalFieldTestHelper(MockCSVParser parser, Transaction transaction, string optionalValue, Action<Transaction> checkParseResult)
    {
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

    [Theory, MemberData(nameof(GenerateLegalRawDataAndTransactionsWithAllOptionalFields))]
    public void TransactionWithAllFieldsCanBeParsed(string rawData, Transaction transaction)
    {
        var parser = new MockCSVParser(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, MockCSVParser.DefaultSeparator);

        var parseResult = parser.Parse(rawData);

        parseResult.Amount.ShouldBe(transaction.Amount);
        parseResult.Currency.ShouldBe(transaction.Currency);
        parseResult.ThisPartyIdentifier.ShouldBe(transaction.ThisPartyIdentifier);
        parseResult.ThisPartyName.ShouldBe(transaction.ThisPartyName);
        parseResult.OtherPartyIdentifier.ShouldBe(transaction.OtherPartyIdentifier);
        parseResult.OtherPartyName.ShouldBe(transaction.OtherPartyName);
        parseResult.Timestamp.ShouldBe(transaction.Timestamp);
        parseResult.PaymentReference.ShouldBe(transaction.PaymentReference);
        parseResult.BIC.ShouldBe(transaction.BIC);
        parseResult.Description.ShouldBe(transaction.Description);
        parseResult.RawData.ShouldBe(transaction.RawData);
    }


    [Theory, CombinatorialData]
    public void DifferentSeparatorsAlsoWork(
        [CombinatorialMemberData(nameof(GenerateLegalTransactions))] Transaction transaction,
        [CombinatorialValues(",", "SEPARATOR", "Any arbitrary string should just work")] string separator)
    {
        var parser = new MockCSVParser(separator);
        var rawData = GenerateBasicRawDataLine(transaction, separator);

        var parseResult = parser.Parse(rawData);
        parseResult.Amount.ShouldBe(transaction.Amount);
        parseResult.Currency.ShouldBe(transaction.Currency);
        parseResult.ThisPartyIdentifier.ShouldBe(transaction.ThisPartyIdentifier);
        parseResult.OtherPartyIdentifier.ShouldBe(transaction.OtherPartyIdentifier);
        parseResult.Timestamp.ShouldBe(transaction.Timestamp);
    }

    public static IEnumerable<object[]> GenerateLegalTransactions()
        => GenerateLegalTransactionsWithCSVRawData().Select(transaction => new object[] { transaction });
    public static IEnumerable<object[]> GenerateLegalRawDataAndTransactions()
        => GenerateLegalTransactionsWithCSVRawData().Select(transaction => new object[] { transaction.RawData, transaction });
    public static IEnumerable<object[]> GenerateLegalRawDataAndTransactionsWithAllOptionalFields()
        => GenerateAllFieldsLegalTransactions(transaction => GenerateBasicRawDataLineWithAllOptionalFields(
                                    transaction.Amount.ToString(),
                                    transaction.Currency.ToString(),
                                    transaction.ThisPartyIdentifier,
                                    transaction.ThisPartyName,
                                    transaction.OtherPartyIdentifier,
                                    transaction.OtherPartyName,
                                    transaction.Timestamp.ToString(),
                                    transaction.PaymentReference!,
                                    transaction.BIC!,
                                    transaction.Description!))
                .Select(transaction => new object[] { transaction.RawData, transaction });

    private static IEnumerable<Transaction> GenerateLegalTransactionsWithCSVRawData()
        => GenerateBasicLegalTransactions(transaction => GenerateBasicRawDataLine(transaction, MockCSVParser.DefaultSeparator));

    private static string GenerateBasicRawDataLine(Transaction transaction, string separator)
        => GenerateBasicRawDataLine(transaction.Amount.ToString(), transaction.Currency.ToString(), transaction.ThisPartyIdentifier, transaction.OtherPartyIdentifier, transaction.Timestamp.ToString(), separator);
    private static string GenerateBasicRawDataLine(string amount, string currency, string thisPartyIdentifier, string otherPartyIdentifier, string timestamp, string separator = MockCSVParser.DefaultSeparator)
        => GenerateSeperatedValueString(separator, amount, currency, thisPartyIdentifier, otherPartyIdentifier, timestamp);
    private static string GenerateBasicRawDataLineWithOptionalField(string amount, string currency, string thisPartyIdentifier, string otherPartyIdentifier, string timestamp, string optionalFieldValue, string separator = MockCSVParser.DefaultSeparator)
        => GenerateSeperatedValueString(separator, amount, currency, thisPartyIdentifier, otherPartyIdentifier, timestamp, optionalFieldValue);

    private static string GenerateBasicRawDataLineWithAllOptionalFields(
        string amount,
        string currency,
        string thisPartyIdentifier,
        string thisPartyName,
        string otherPartyIdentifier,
        string otherPartyName,
        string timestamp,
        string paymentReference,
        string bic,
        string description,
        string separator = MockCSVParser.DefaultSeparator)
        => GenerateSeperatedValueString(separator, amount, currency, thisPartyIdentifier, thisPartyName, otherPartyIdentifier, otherPartyName, timestamp, paymentReference, bic, description);

    private static string GenerateSeperatedValueString(string seperator, params string[] values) => string.Join(seperator, values);

    public static string[] GetExamplesFor(string fieldName) => fieldName switch
    {
        nameof(Transaction.ThisPartyName) => ThisPartyNameExamples(),
        nameof(Transaction.OtherPartyName) => OtherPartyNameExamples(),
        nameof(Transaction.PaymentReference) => PaymentReferenceExamples(),
        nameof(Transaction.BIC) => BICExamples(),
        nameof(Transaction.Description) => DescriptionExamples(),
        _ => throw new NotImplementedException(),
    };
}