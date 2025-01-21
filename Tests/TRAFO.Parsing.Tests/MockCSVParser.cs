namespace TRAFO.Parsing.Tests;

public class MockCSVParser : CSVParser
{
    public MockCSVParser() : this(0, 1, 2, 3, DefaultSeparator) { }
    public MockCSVParser(string separator) : this(0, 1, 2, 3, separator) { }
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