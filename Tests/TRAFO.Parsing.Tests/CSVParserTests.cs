namespace TRAFO.Parsing.Tests;

public class CSVParserTests
{
    [Fact]
    public void Test1()
    {

    }
}

public class MockCSVParser : CSVParser
{
    public MockCSVParser() : base(new CSVParserConfiguration
    {
        AmountIndex = 0,
        CurrencyIndex = 1,
        OtherPartyNameIndex = 2,
        TimestampIndex = 3,
        Separator = ",",
    }) { }
}
