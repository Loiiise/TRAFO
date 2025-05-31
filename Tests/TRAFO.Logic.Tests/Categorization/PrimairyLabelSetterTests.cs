using Shouldly;
using TRAFO.Logic.Categorization;
using TRAFO.Logic.Categorization.Predicates;
using TRAFO.Logic.Dto;

namespace TRAFO.Logic.Tests.Categorization;

public class PrimairyLabelSetterTests
{
    [Theory, MemberData(nameof(GenerateLegalTransactions))]
    public void AlwaysTruePredicateIsAlwaysApplied(Transaction transaction)
    {
        var primairyLabel = "I'm always set";
        var alwaysTruePredicate = new CustomPredicate()
        {
            Predicate = _ => true,
            LabelToSet = primairyLabel,
        };

        var predicates = new TransactionPredicate[] { alwaysTruePredicate };
        var primairyLabelSetter = new TransactionLabelSetter();

        var result = primairyLabelSetter.ApplyPredicates(transaction, predicates);

        result.PrimairyLabel.ShouldBe(primairyLabel);
    }

    [Theory, MemberData(nameof(GenerateLegalTransactions))]
    public void AlwaysFalsePredicatesAreNeverApplied(Transaction transaction)
    {
        var primairyLabel = "Don't you dare setting me";
        var alwaysFalsePredicate = new CustomPredicate()
        {
            Predicate = _ => false,
            LabelToSet = primairyLabel,
        };

        var predicates = new TransactionPredicate[] { alwaysFalsePredicate };
        var primairyLabelSetter = new TransactionLabelSetter();

        var result = primairyLabelSetter.ApplyPredicates(transaction, predicates);

        result.PrimairyLabel.ShouldBeNull();
    }

    [Theory, MemberData(nameof(GenerateLegalTransactions))]
    public void MultiplePassingPredicatesResultsInTheFirstOneBeingApplied(Transaction transaction)
    {
        var labelZero = "Label 0";
        var alwaysLabelZeroPredicate = new CustomPredicate()
        {
            Predicate = _ => true,
            LabelToSet = labelZero,
        };

        var labelOne = "Label 1";
        var alwaysLabelOnePredicate = new CustomPredicate()
        {
            Predicate = _ => true,
            LabelToSet = labelOne,
        };

        var zeroFirstPredicates = new TransactionPredicate[] { alwaysLabelZeroPredicate, alwaysLabelOnePredicate };
        var oneFirstPredicates = new TransactionPredicate[] { alwaysLabelOnePredicate, alwaysLabelZeroPredicate };
        var primairyLabelSetter = new TransactionLabelSetter();

        var zeroFirstResult = primairyLabelSetter.ApplyPredicates(transaction, zeroFirstPredicates);
        zeroFirstResult.PrimairyLabel.ShouldBe(labelZero);
        zeroFirstResult.PrimairyLabel.ShouldNotBe(labelOne);

        var oneFirstResult = primairyLabelSetter.ApplyPredicates(transaction, oneFirstPredicates);
        oneFirstResult.PrimairyLabel.ShouldNotBe(labelZero);
        oneFirstResult.PrimairyLabel.ShouldBe(labelOne);
    }

    public static IEnumerable<object[]> GenerateLegalTransactions()
        => TransactionFixture.GenerateBasicLegalTransactionsWithoutRawData().Select(transaction => new object[] { transaction });
}
