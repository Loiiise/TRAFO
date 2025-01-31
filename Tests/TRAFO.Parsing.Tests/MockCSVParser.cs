namespace TRAFO.Parsing.Tests;

public class MockCSVParser : CSVParser
{
    public MockCSVParser() : this(0, 1, 2, 3, 4, DefaultSeparator) { }
    public MockCSVParser(string separator) : this(0, 1, 2, 3, 4, separator) { }
    public MockCSVParser(int amountIndex, int currencyIndex, int thisPartyIdentifierIndex, int otherPartyIdentifierIndex, int timestampIndex) : this(amountIndex, currencyIndex, thisPartyIdentifierIndex, otherPartyIdentifierIndex, timestampIndex, DefaultSeparator) { }

    public MockCSVParser(int amountIndex, int currencyIndex, int thisPartyIdentifierIndex, int otherPartyIdentifierIndex, int timestampIndex, string separator) : base(new CSVParserConfiguration
    {
        AmountIndex = amountIndex,
        CurrencyIndex = currencyIndex,
        ThisPartyIdentifierIndex = thisPartyIdentifierIndex,
        OtherPartyIdentifierIndex = otherPartyIdentifierIndex,
        TimestampIndex = timestampIndex,
        Separator = separator,
    })
    { }

    public const string DefaultSeparator = ";";
}