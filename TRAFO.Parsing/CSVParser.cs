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

        // Check validity of manditory indices
        if (_configuration.AmountIndex >= amountOfItems) exception = CSVParserConfigurationIndexOutOfRange(nameof(_configuration.AmountIndex), _configuration.AmountIndex, amountOfItems);
        if (_configuration.CurrencyIndex >= amountOfItems) exception = CSVParserConfigurationIndexOutOfRange(nameof(_configuration.CurrencyIndex), _configuration.CurrencyIndex, amountOfItems);
        if (_configuration.ThisPartyIdentifierIndex >= amountOfItems) exception = CSVParserConfigurationIndexOutOfRange(nameof(_configuration.ThisPartyIdentifierIndex), _configuration.ThisPartyIdentifierIndex, amountOfItems);
        if (_configuration.OtherPartyIdentifierIndex >= amountOfItems) exception = CSVParserConfigurationIndexOutOfRange(nameof(_configuration.OtherPartyIdentifierIndex), _configuration.OtherPartyIdentifierIndex, amountOfItems);
        if (_configuration.TimestampIndex >= amountOfItems) exception = CSVParserConfigurationIndexOutOfRange(nameof(_configuration.TimestampIndex), _configuration.TimestampIndex, amountOfItems);

        // Parse mandatory non string fields
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

        // Get optional fields. These are null if the index is not specified or out of range
        string?
            thisPartyName = GetItemIfIndexValid(_configuration.ThisPartyNameIndex),
            otherPartyName = GetItemIfIndexValid(_configuration.OtherPartyNameIndex),
            paymentReference = GetItemIfIndexValid(_configuration.PaymentReferenceIndex),
            bic = GetItemIfIndexValid(_configuration.BICIndex),
            description = GetItemIfIndexValid(_configuration.DescriptionIndex);

        // Stop if anything went wrong
        if (exception != null)
        {
            transaction = null;
            return false;
        }

        // Make the base transaction with previously parsed fields
        transaction = new Transaction
        {
            Amount = amount,
            Currency = currency,
            ThisPartyIdentifier = items[_configuration.ThisPartyIdentifierIndex],
            OtherPartyIdentifier = items[_configuration.OtherPartyIdentifierIndex],
            Timestamp = timestamp,
            PaymentReference = paymentReference,
            BIC = bic,
            RawData = line,
            Labels = Array.Empty<string>(),
        };

        // Set fields that are not nullable, but are optional to set
        if (thisPartyName is not null) transaction = transaction with { ThisPartyName = thisPartyName };
        if (otherPartyName is not null) transaction = transaction with { OtherPartyName = otherPartyName };
        if (description is not null) transaction = transaction with { Description = description };

        return true;

        string? GetItemIfIndexValid(int? index)
            => index is not null && index < amountOfItems
                ? items[(int)index]
                : null;
    }

    private IndexOutOfRangeException CSVParserConfigurationIndexOutOfRange(string indexName, int indexValue, int upperLimit)
        => new IndexOutOfRangeException($"{nameof(indexName)} was out of range. Was {indexValue}, but should have been smaller than {upperLimit}.");

    private readonly CSVParserConfiguration _configuration;

    protected sealed record CSVParserConfiguration
    {
        public required int AmountIndex { get; init; }
        public required int CurrencyIndex { get; init; }
        public required int ThisPartyIdentifierIndex { get; init; }
        public int? ThisPartyNameIndex { get; init; }
        public required int OtherPartyIdentifierIndex { get; init; }
        public int? OtherPartyNameIndex { get; init; }
        public required int TimestampIndex { get; init; }
        public int? PaymentReferenceIndex { get; init; }
        public int? BICIndex { get; init; }
        public int? DescriptionIndex { get; init; }
        public required string Separator { get; init; }
    }
}