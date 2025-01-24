using System.Diagnostics.CodeAnalysis;
using TRAFO.Logic;

namespace TRAFO.Parsing;
public abstract class CSVParser : Parser
{
    protected CSVParser(CSVParserConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected sealed override bool ParseSafe(string line, [MaybeNullWhen(false)] out Transaction? transaction, [MaybeNullWhen(true)] out Exception exception)
    {
        exception = null;

        var items = line.Split(_configuration.Separator);
        var amountOfItems = items.Length;

        if (_configuration.AmountIndex >= amountOfItems) exception = CSVParserConfigurationIndexOutOfRange(nameof(_configuration.AmountIndex), _configuration.AmountIndex, amountOfItems);
        if (_configuration.CurrencyIndex >= amountOfItems) exception = CSVParserConfigurationIndexOutOfRange(nameof(_configuration.CurrencyIndex), _configuration.CurrencyIndex, amountOfItems);
        if (_configuration.ThisPartyIdentifierIndex >= amountOfItems) exception = CSVParserConfigurationIndexOutOfRange(nameof(_configuration.ThisPartyIdentifierIndex), _configuration.ThisPartyIdentifierIndex, amountOfItems);
        if (_configuration.OtherPartyIdentifierIndex >= amountOfItems) exception = CSVParserConfigurationIndexOutOfRange(nameof(_configuration.OtherPartyIdentifierIndex), _configuration.OtherPartyIdentifierIndex, amountOfItems);
        if (_configuration.TimestampIndex >= amountOfItems) exception = CSVParserConfigurationIndexOutOfRange(nameof(_configuration.TimestampIndex), _configuration.TimestampIndex, amountOfItems);

        if (!long.TryParse(items[_configuration.AmountIndex], out var amount))
        {
            exception = new ArgumentException($"{items[_configuration.AmountIndex]} should be the amount, but was not a number.");
        }
        if (!Enum.TryParse<Currency>(items[_configuration.CurrencyIndex], out var currency))
        {
            exception = new ArgumentException($"{items[_configuration.CurrencyIndex]} should be a currency, but wasn't parsed as such.");
        }
        if (!DateTime.TryParse(items[_configuration.TimestampIndex], out var timestamp))
        {
            exception = new ArgumentException($"{items[_configuration.TimestampIndex]} should be a timestamp, but wasn't parsed as such.");
        }

        if (exception != null)
        {
            transaction = null;
            return false;
        }

        transaction = new Transaction
        {
            Amount = amount,
            Currency = currency,
            ThisPartyIdentifier = items[_configuration.ThisPartyIdentifierIndex],
            OtherPartyIdentifier = items[_configuration.OtherPartyIdentifierIndex],
            Timestamp = timestamp,
            RawData = line,
            Labels = Array.Empty<string>(),
        };
        return true;
    }

    private IndexOutOfRangeException CSVParserConfigurationIndexOutOfRange(string indexName, int indexValue, int upperLimit)
        => new IndexOutOfRangeException($"{nameof(indexName)} was out of range. Was {indexValue}, but should have been smaller than {upperLimit}.");

    private readonly CSVParserConfiguration _configuration;

    protected record CSVParserConfiguration
    {
        public int AmountIndex { get; init; }
        public int CurrencyIndex { get; init; }
        public int ThisPartyIdentifierIndex { get; init; }
        public int? ThisPartyNameIndex { get; init; }
        public int OtherPartyIdentifierIndex { get; init; }
        public int? OtherPartyNameIndex { get; init; }
        public int TimestampIndex { get; init; }
        public int? DescriptionIndex { get; init; }
        public required string Separator { get; init; }
    }
}