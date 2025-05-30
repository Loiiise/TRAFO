using TRAFO.Services.Parser.CSV;

namespace TRAFO.Services.Parser.Tests;

public class MockCSVParser : CSVParser
{
    public MockCSVParser() : this(GenerateConfiguration()) { }
    public MockCSVParser(string separator) : this(GenerateConfiguration(separator)) { }
    public MockCSVParser(int amountIndex, int currencyIndex, int thisPartyIdentifierIndex, int otherPartyIdentifierIndex, int timestampIndex)
        : this(GenerateConfiguration(amountIndex, currencyIndex, thisPartyIdentifierIndex, otherPartyIdentifierIndex, timestampIndex, DefaultSeparator)) { }

    public MockCSVParser(int amountIndex, int currencyIndex, int thisPartyIdentifierIndex, int otherPartyIdentifierIndex, int timestampIndex, string separator)
        : this(GenerateConfiguration(amountIndex, currencyIndex, thisPartyIdentifierIndex, otherPartyIdentifierIndex, timestampIndex, separator))
    { }

    public MockCSVParser(
        int amountIndex,
        int currencyIndex,
        int thisPartyIdentifierIndex,
        int? thisPartyNameIndex,
        int otherPartyIdentifierIndex,
        int? otherPartyNameIndex,
        int timestampIndex,
        int? PaymentReferenceIndex,
        int? BICIndex,
        int? DescriptionIndex,
        string separator) : this(new CSVParserConfiguration
        {
            AmountIndex = amountIndex,
            CurrencyIndex = currencyIndex,
            ThisPartyIdentifierIndex = thisPartyIdentifierIndex,
            ThisPartyNameIndex = thisPartyNameIndex,
            OtherPartyIdentifierIndex = otherPartyIdentifierIndex,
            OtherPartyNameIndex = otherPartyNameIndex,
            TimestampIndex = timestampIndex,
            PaymentReferenceIndex = PaymentReferenceIndex,
            BICIndex = BICIndex,
            DescriptionIndex = DescriptionIndex,
            Separator = separator,
        })
    { }


    private MockCSVParser(CSVParserConfiguration configuration) : base(configuration) { }

    public static MockCSVParser GetBasicCSVParserWithThisPartyNameIndex() => new MockCSVParser(GenerateConfiguration() with { ThisPartyNameIndex = _firstAvailableIndex });
    public static MockCSVParser GetBasicCSVParserWithOtherPartyNameIndex() => new MockCSVParser(GenerateConfiguration() with { OtherPartyNameIndex = _firstAvailableIndex });
    public static MockCSVParser GetBasicCSVParserWithPaymentReferenceIndex() => new MockCSVParser(GenerateConfiguration() with { PaymentReferenceIndex = _firstAvailableIndex });
    public static MockCSVParser GetBasicCSVParserWithBICIndex() => new MockCSVParser(GenerateConfiguration() with { BICIndex = _firstAvailableIndex });
    public static MockCSVParser GetBasicCSVParserWithDescriptionIndex() => new MockCSVParser(GenerateConfiguration() with { DescriptionIndex = _firstAvailableIndex });

    public const string DefaultSeparator = ";";

    private static CSVParserConfiguration GenerateConfiguration(string separator = DefaultSeparator)
        => GenerateConfiguration(0, 1, 2, 3, 4, separator);
    private static CSVParserConfiguration GenerateConfiguration(int amountIndex, int currencyIndex, int thisPartyIdentifierIndex, int otherPartyIdentifierIndex, int timestampIndex, string separator) => new CSVParserConfiguration
    {
        AmountIndex = amountIndex,
        CurrencyIndex = currencyIndex,
        ThisPartyIdentifierIndex = thisPartyIdentifierIndex,
        OtherPartyIdentifierIndex = otherPartyIdentifierIndex,
        TimestampIndex = timestampIndex,
        Separator = separator,
    };

    private const int _firstAvailableIndex = 5;
}