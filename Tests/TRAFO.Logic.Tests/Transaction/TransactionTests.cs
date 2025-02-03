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
    public void ToStringContainsAmountCurrenyTimestampAndPartyIdentifiers()
    {
        foreach (var transaction in GenerateBasicLegalTransactionsWithoutRawData())
        {
            transaction.ToString().ShouldContain(transaction.Amount.ToString());
            transaction.ToString().ShouldContain(transaction.Currency.ToString());
            transaction.ToString().ShouldContain(transaction.ThisPartyIdentifier);
            transaction.ToString().ShouldContain(transaction.OtherPartyIdentifier);
            transaction.ToString().ShouldContain(transaction.Timestamp.ToString());
        }
    }
}
