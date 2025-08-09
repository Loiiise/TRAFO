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

        var transactionWithOnlyThisPartyIdentifier = GetEmptyTransaction() with { ThisAccountIdentifier = thisPartyIdentifier };

        transactionWithOnlyThisPartyIdentifier.ThisAccountIdentifier.ShouldBe(thisPartyIdentifier);
        transactionWithOnlyThisPartyIdentifier.ThisAccountName.ShouldBe(thisPartyIdentifier);

        var transactionWithThisPartyIdentifierAndThisPartyName = transactionWithOnlyThisPartyIdentifier with { ThisAccountName = thisPartyName };

        transactionWithThisPartyIdentifierAndThisPartyName.ThisAccountIdentifier.ShouldBe(thisPartyIdentifier);
        transactionWithThisPartyIdentifierAndThisPartyName.ThisAccountName.ShouldBe(thisPartyName);
    }

    [Fact]
    public void OtherPartyNameShouldBeOtherPartyIdentifierIfNotSet()
    {
        var otherPartyIdentifier = "Identify you";
        var otherPartyName = "Your name";

        var transactionWithOnlyOtherPartyIdentifier = GetEmptyTransaction() with { OtherAccountIdentifier = otherPartyIdentifier };

        transactionWithOnlyOtherPartyIdentifier.OtherAccountIdentifier.ShouldBe(otherPartyIdentifier);
        transactionWithOnlyOtherPartyIdentifier.OtherAccountName.ShouldBe(otherPartyIdentifier);

        var transactionWithOtherPartyIdentifierAndOtherPartyName = transactionWithOnlyOtherPartyIdentifier with { OtherAccountName = otherPartyName };

        transactionWithOtherPartyIdentifierAndOtherPartyName.OtherAccountIdentifier.ShouldBe(otherPartyIdentifier);
        transactionWithOtherPartyIdentifierAndOtherPartyName.OtherAccountName.ShouldBe(otherPartyName);
    }

    [Fact]
    public void ToStringContainsAmountCurrenyTimestampAndPartyIdentifiers()
    {
        foreach (var transaction in GenerateBasicLegalTransactionsWithoutRawData())
        {
            foreach (var digitOrSign in transaction.Amount.ToString())
            {
                transaction.ToString().ShouldContain(digitOrSign);
            }
            transaction.ToString().ShouldContain(transaction.Currency.ToString());
            transaction.ToString().ShouldContain(transaction.ThisAccountIdentifier);
            transaction.ToString().ShouldContain(transaction.OtherAccountIdentifier);
            transaction.ToString().ShouldContain(transaction.Timestamp.ToString());
        }
    }
}
