using Shouldly;
using static TRAFO.Logic.Tests.TransactionFixture;

namespace TRAFO.Logic.Tests;

public class TransactionTests
{
    [Fact]
    public void ThisPartyNameShouldBeThisPartyIdentifierIfNotSet()
    {
        var thisPartyIdentifier = "Identify me";
        var thisPartyName = "My name";

        var transactionWithOnlyThisPartyIdentifier = GetEmptyTransaction() with { ThisPartyIdentifier = thisPartyIdentifier };

        transactionWithOnlyThisPartyIdentifier.ThisPartyIdentifier.ShouldBe(thisPartyIdentifier);
        transactionWithOnlyThisPartyIdentifier.ThisPartyName.ShouldBe(thisPartyIdentifier);

        var transactionWithThisPartyIdentifierAndThisPartyName = transactionWithOnlyThisPartyIdentifier with { ThisPartyName = thisPartyName };

        transactionWithThisPartyIdentifierAndThisPartyName.ThisPartyIdentifier.ShouldBe(thisPartyIdentifier);
        transactionWithThisPartyIdentifierAndThisPartyName.ThisPartyName.ShouldBe(thisPartyName);
    }

    [Fact]
    public void OtherPartyNameShouldBeOtherPartyIdentifierIfNotSet()
    {
        var otherPartyIdentifier = "Identify you";
        var otherPartyName = "Your name";

        var transactionWithOnlyOtherPartyIdentifier = GetEmptyTransaction() with { OtherPartyIdentifier = otherPartyIdentifier };

        transactionWithOnlyOtherPartyIdentifier.OtherPartyIdentifier.ShouldBe(otherPartyIdentifier);
        transactionWithOnlyOtherPartyIdentifier.OtherPartyName.ShouldBe(otherPartyIdentifier);

        var transactionWithOtherPartyIdentifierAndOtherPartyName = transactionWithOnlyOtherPartyIdentifier with { OtherPartyName = otherPartyName };

        transactionWithOtherPartyIdentifierAndOtherPartyName.OtherPartyIdentifier.ShouldBe(otherPartyIdentifier);
        transactionWithOtherPartyIdentifierAndOtherPartyName.OtherPartyName.ShouldBe(otherPartyName);
    }

    [Fact]
    public void DescriptionContainsAmountCurrenyTimestampAndPartyIdentifierIfNotSet()
    {
        var customDescription = "Not so descriptive";

        foreach (var transaction in GenerateBasicLegalTransactionsWithoutRawData())
        {
            transaction.Description.ShouldContain(transaction.Amount.ToString());
            transaction.Description.ShouldContain(transaction.Currency.ToString());
            transaction.Description.ShouldContain(transaction.ThisPartyIdentifier);
            transaction.Description.ShouldContain(transaction.OtherPartyIdentifier);
            transaction.Description.ShouldContain(transaction.Timestamp.ToString());

            var transactionWithCustomDescription = transaction with { Description = customDescription };

            transactionWithCustomDescription.Description.ShouldNotContain(transactionWithCustomDescription.Amount.ToString());
            transactionWithCustomDescription.Description.ShouldNotContain(transactionWithCustomDescription.Currency.ToString());
            transactionWithCustomDescription.Description.ShouldNotContain(transactionWithCustomDescription.ThisPartyIdentifier);
            transactionWithCustomDescription.Description.ShouldNotContain(transactionWithCustomDescription.OtherPartyIdentifier);
            transactionWithCustomDescription.Description.ShouldNotContain(transactionWithCustomDescription.Timestamp.ToString());

            transactionWithCustomDescription.Description.ShouldBe(customDescription);
        }
    }
}
