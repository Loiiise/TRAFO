namespace TRAFO.Services.Parser.CSV;

public class CustomCSVParser : CSVParser
{
    public CustomCSVParser(
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
        string separator) : base(new CSVParserConfiguration
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
}
