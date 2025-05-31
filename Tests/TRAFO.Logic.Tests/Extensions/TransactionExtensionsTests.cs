using Shouldly;
using TRAFO.Logic.Extensions;
using static TRAFO.Logic.Tests.TransactionFixture;

namespace TRAFO.Logic.Tests.Extensions;
public class TransactionExtensionsTests
{
    [Fact]
    public void ALotOfUniqueLabelsCanBeAdded()
    {
        var transaction = GetEmptyTransaction();
        transaction.Labels.ShouldBeEmpty();

        for (int i = 0; i <= 1_000; i++)
        {
            transaction.Labels.Length.ShouldBe(i);

            var label = "This is label #" + i;
            transaction = transaction.AddLabel(label);

            transaction.Labels.Length.ShouldBe(i + 1);
            transaction.Labels[i].ShouldBe(label);
        }
    }

    [Fact]
    public void LabelsShouldNeverContainDuplicates()
    {
        var label = "I'm a basic label";
        var sameLabel = label;
        var otherLabel = "I'm not like other girls";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithMultipleUniqueLabels = emptyTransaction.AddLabels(label, otherLabel);
        transactionWithMultipleUniqueLabels.Labels.ShouldContain(label);
        transactionWithMultipleUniqueLabels.Labels.ShouldContain(otherLabel);
        transactionWithMultipleUniqueLabels.Labels.Length.ShouldBe(2);

        var transactionWithMultipleUniqueLabelsAttemptToAddDuplicate = transactionWithMultipleUniqueLabels.AddLabels(sameLabel);
        transactionWithMultipleUniqueLabelsAttemptToAddDuplicate.ShouldBe(transactionWithMultipleUniqueLabels);
        transactionWithMultipleUniqueLabelsAttemptToAddDuplicate.Labels.ShouldContain(label);
        transactionWithMultipleUniqueLabels.Labels.ShouldContain(otherLabel);
        transactionWithMultipleUniqueLabelsAttemptToAddDuplicate.Labels.Length.ShouldBe(2);

        var transactionWithOnlyDuplicateLabels = emptyTransaction.AddLabels(label, sameLabel);
        transactionWithOnlyDuplicateLabels.Labels.ShouldContain(label);
        transactionWithOnlyDuplicateLabels.Labels.ShouldNotContain(otherLabel);
        transactionWithOnlyDuplicateLabels.Labels.Length.ShouldBe(1);

        var transactionWithDuplicateAndOtherLabels = emptyTransaction.AddLabels(label, sameLabel, otherLabel);
        transactionWithDuplicateAndOtherLabels.Labels.ShouldContain(label);
        transactionWithDuplicateAndOtherLabels.Labels.ShouldContain(otherLabel);
        transactionWithDuplicateAndOtherLabels.Labels.Length.ShouldBe(2);

    }

    [Fact]
    public void RemoveLabelTest()
    {
        var label = "This is a label";
        var otherLabel = "This is another label";

        var transaction = GetEmptyTransaction()
            .AddLabel(label)
            .AddLabel(otherLabel);

        transaction.Labels.Length.ShouldBe(2);
        transaction.Labels.ShouldContain(label);
        transaction.Labels.ShouldContain(otherLabel);

        transaction = transaction.RemoveLabel(label);
        transaction.Labels.Length.ShouldBe(1);
        transaction.Labels.ShouldNotContain(label);
        transaction.Labels.ShouldContain(otherLabel);

        for (var i = 0; i < 100; i++)
        {
            transaction = transaction.RemoveLabel(label);
            transaction.Labels.Length.ShouldBe(1);
            transaction.Labels.ShouldNotContain(label);
            transaction.Labels.ShouldContain(otherLabel);
        }

        transaction = transaction.RemoveLabel(otherLabel);
        transaction.Labels.Length.ShouldBe(0);
        transaction.Labels.ShouldNotContain(label);
        transaction.Labels.ShouldNotContain(otherLabel);

        for (var i = 0; i < 100; i++)
        {
            transaction = transaction.RemoveLabel(label);
            transaction = transaction.RemoveLabel(otherLabel);
            transaction.Labels.Length.ShouldBe(0);
            transaction.Labels.ShouldNotContain(label);
            transaction.Labels.ShouldNotContain(otherLabel);
        }
    }

    [Fact]
    public void RemoveAllLabelsShouldRemoveThemAll()
    {
        var FirstLabel = "This is such a superior label";
        var basicLabel = "This is a label";
        var otherLabel = "This is another label";

        var transactionWithLabels = GetEmptyTransaction()
            .AddLabel(FirstLabel)
            .AddLabel(basicLabel)
            .AddLabel(otherLabel);

        transactionWithLabels.Labels.Length.ShouldBe(3);
        transactionWithLabels.Labels.ShouldContain(FirstLabel);
        transactionWithLabels.Labels.ShouldContain(basicLabel);
        transactionWithLabels.Labels.ShouldContain(otherLabel);

        var transactionWithoutAnyLabels = transactionWithLabels.RemoveAllLabels();
        transactionWithoutAnyLabels.Labels.Length.ShouldBe(0);
        transactionWithoutAnyLabels.Labels.ShouldNotContain(FirstLabel);
        transactionWithoutAnyLabels.Labels.ShouldNotContain(basicLabel);
        transactionWithoutAnyLabels.Labels.ShouldNotContain(otherLabel);
    }
}
