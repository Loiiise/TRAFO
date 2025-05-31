using Shouldly;
using TRAFO.Logic.Categorization;
using TRAFO.Logic.Categorization.Predicates;
using TRAFO.Logic.Dto;

namespace TRAFO.Logic.Tests.Categorization;

public class TransactionLabelSetterTests
{
    [Theory, MemberData(nameof(GenerateLegalTransactions))]
    public void AlwaysTruePredicateIsAlwaysApplied(Transaction transaction)
    {
        var label = "I'm always set";
        var alwaysTruePredicate = new CustomPredicate()
        {
            Predicate = _ => true,
            LabelToSet = label,
        };

        var predicates = new TransactionPredicate[] { alwaysTruePredicate };
        var labelSetter = new TransactionLabelSetter();

        var result = labelSetter.ApplyPredicates(transaction, predicates);

        result.Labels.ShouldContain(label);
        result.Labels.ShouldBe(new string[] { label });
    }

    [Theory, MemberData(nameof(GenerateLegalTransactions))]
    public void AlwaysFalsePredicatesAreNeverApplied(Transaction transaction)
    {
        var label = "Don't you dare setting me";
        var alwaysFalsePredicate = new CustomPredicate()
        {
            Predicate = _ => false,
            LabelToSet = label,
        };

        var predicates = new TransactionPredicate[] { alwaysFalsePredicate };
        var labelSetter = new TransactionLabelSetter();

        var result = labelSetter.ApplyPredicates(transaction, predicates);

        result.Labels.ShouldBeEmpty();
    }

    [Theory, MemberData(nameof(GenerateLegalTransactions))]
    public void MultiplePassingPredicatesAreAppliedInOrder(Transaction transaction)
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
        var labelSetter = new TransactionLabelSetter();

        var zeroFirstResult = labelSetter.ApplyPredicates(transaction, zeroFirstPredicates);
        zeroFirstResult.Labels.ShouldContain(labelZero);
        zeroFirstResult.Labels.ShouldBe(new string[] { labelZero, labelOne });

        var oneFirstResult = labelSetter.ApplyPredicates(transaction, oneFirstPredicates);
        zeroFirstResult.Labels.ShouldContain(labelOne);
        zeroFirstResult.Labels.ShouldBe(new string[] { labelOne, labelZero });
    }

    public static IEnumerable<object[]> GenerateLegalTransactions()
        => TransactionFixture.GenerateBasicLegalTransactionsWithoutRawData().Select(transaction => new object[] { transaction });
}
