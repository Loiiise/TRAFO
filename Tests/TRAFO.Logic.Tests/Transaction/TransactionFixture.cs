using TRAFO.Logic.Dto;

namespace TRAFO.Logic.Tests;
public static class TransactionFixture
{
    public static Transaction GetEmptyTransaction() => new Transaction
    {
        Amount = 0,
        Currency = Currency.EUR,
        ThisAccountIdentifier = string.Empty,
        OtherAccountIdentifier = string.Empty,
        Timestamp = new DateTime(1970, 01, 01),
        RawData = string.Empty,
        Labels = Array.Empty<string>(),
    };

    public static Transaction GenerateOneBasicLegalTransactionWithoutRawData() => GenerateOneBasicLegalTransaction(_ => string.Empty);
    public static Transaction GenerateOneBasicLegalTransaction(Func<Transaction, string> generateRawData) => GenerateBasicLegalTransactions(generateRawData).First();

    public static IEnumerable<Transaction> GenerateBasicLegalTransactionsWithoutRawData() => GenerateBasicLegalTransactions(_ => string.Empty);
    public static IEnumerable<Transaction> GenerateBasicLegalTransactions(Func<Transaction, string> generateRawData)
    {
        foreach (var amount in new long[] { 23, 12, -504, 1028 })
            foreach (var currency in new Currency[] { Currency.EUR, Currency.USD })
                foreach (string thisPartyIdentifier in new[] { "THIS PARTY", "me", "myself" })
                    foreach (string otherPartyIdentifier in new[] { "OTHER PARTY", "John Doe", "Jack Sparrow" })
                        foreach (var timestamp in new[] { new DateTime(2025, 01, 20, 18, 39, 12), new DateTime(2022, 12, 26, 06, 52, 37) })
                            foreach (var labels in new[] { Array.Empty<string>(), new[] { "label0", "i dont wanna be a label" } })
                            {
                                var transaction = new Transaction
                                {
                                    Amount = amount,
                                    Currency = currency,
                                    ThisAccountIdentifier = thisPartyIdentifier,
                                    OtherAccountIdentifier = otherPartyIdentifier,
                                    Timestamp = timestamp,
                                    RawData = string.Empty,
                                    Labels = labels,
                                };

                                yield return transaction with { RawData = generateRawData(transaction) };
                            }
    }

    public static IEnumerable<Transaction> GenerateAllFieldsLegalTransactions(Func<Transaction, string> generateRawData)
    {
        var transactionWithMandatoryFields = GenerateOneBasicLegalTransactionWithoutRawData();

        foreach (var thisPartyName in ThisPartyNameExamples())
            foreach (var otherPartyName in OtherPartyNameExamples())
                foreach (var paymentReference in PaymentReferenceExamples())
                    foreach (var bic in BICExamples())
                        foreach (var description in DescriptionExamples())
                        {
                            var transactionWithAllFields = transactionWithMandatoryFields with
                            {
                                ThisAccountName = thisPartyName,
                                OtherPartyName = otherPartyName,
                                PaymentReference = paymentReference,
                                BIC = bic,
                                Description = description,
                            };

                            yield return transactionWithAllFields with { RawData = generateRawData(transactionWithAllFields) };
                        }
    }

    public static string[] ThisPartyNameExamples() => new[] { "it is I", "we da party" };
    public static string[] OtherPartyNameExamples() => new[] { "opponents", "the others", "they/them" };
    public static string[] PaymentReferenceExamples() => new[] { "Invoice 202500000001", "Order 69420" };
    public static string[] BICExamples() => new[] { "HBMBNL69", "ABCDCC00111" };
    public static string[] DescriptionExamples() => new[] { "blablabla", "I describe", "The Boons and the Banes" };
}
