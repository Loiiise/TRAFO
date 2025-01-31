namespace TRAFO.Parsing.Tests;

public class MockCSVParser : CSVParser
{
    public MockCSVParser() : this(0, 1, 3, 4, DefaultSeparator) { }
    public MockCSVParser(string separator) : this(0, 1, 3, 4, separator) { }
    public MockCSVParser(int amountIndex, int currencyIndex, int otherPartyNameIndex, int timestampIndex) : this(amountIndex, currencyIndex, otherPartyNameIndex, timestampIndex, DefaultSeparator) { }

    public MockCSVParser(int amountIndex, int currencyIndex, int otherPartyNameIndex, int timestampIndex, string separator) : base(new CSVParserConfiguration
    {
        AmountIndex = amountIndex,
        CurrencyIndex = currencyIndex,
        OtherPartyNameIndex = otherPartyNameIndex,
        TimestampIndex = timestampIndex,
        Separator = separator,
    })
    { }

    public const string DefaultSeparator = ";";
}