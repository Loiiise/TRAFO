using Shouldly;

namespace TRAFO.Logic.Tests;

public class TransactionTests
{
    [Fact]
    public void SettingThePrimairyLabelShouldAlsoAddThatLabelToLabels()
    {
        var primairyLabel = "This is such a superior label";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithPrimairyLabel = emptyTransaction with { PrimairyLabel = primairyLabel };
        transactionWithPrimairyLabel.PrimairyLabel.ShouldBe(primairyLabel);
        transactionWithPrimairyLabel.Labels.Length.ShouldBe(1);
        transactionWithPrimairyLabel.Labels[0].ShouldBe(primairyLabel);
    }

    [Fact]
    public void AddingALabelToLabelsShouldNotMakeItPrimairyLabelIfPrimairyLabelHasNotBeenSet()
    {
        var label = "This is a label";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithLabelInLabels = emptyTransaction with { Labels = new[] { label } };
        transactionWithLabelInLabels.Labels.Length.ShouldBe(1);
        transactionWithLabelInLabels.Labels[0].ShouldBe(label);
        transactionWithLabelInLabels.PrimairyLabel.ShouldBeNull();
    }

    [Fact]
    public void AddingALabelToLabelsShouldNotMakeItPrimairyLabelIfPrimairyLabelHasAlreadyBeenSet()
    {
        var label = "This is a label";
        var primairyLabel = "This is such a superior label";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithPrimairyLabel = emptyTransaction with { PrimairyLabel = primairyLabel };
        transactionWithPrimairyLabel.PrimairyLabel.ShouldBe(primairyLabel);
        transactionWithPrimairyLabel.Labels.Length.ShouldBe(1);
        transactionWithPrimairyLabel.Labels[0].ShouldBe(primairyLabel);

        var newLabelCollection = transactionWithPrimairyLabel.Labels.Append(label).ToArray();

        var transactionWithPrimairyLabelAndLabels = transactionWithPrimairyLabel with { Labels = newLabelCollection };
        transactionWithPrimairyLabelAndLabels.PrimairyLabel.ShouldBe(primairyLabel);
        transactionWithPrimairyLabelAndLabels.Labels.Length.ShouldBe(2);
        transactionWithPrimairyLabelAndLabels.Labels[0].ShouldBe(primairyLabel);
        transactionWithPrimairyLabelAndLabels.Labels[1].ShouldBe(label);
    }

    [Fact]
    public void SettingPrimairyLabelToNullShouldNotRemoveItFromLabels()
    {
        var primairyLabel = "This is such a superior label";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithPrimairyLabel = emptyTransaction with { PrimairyLabel = primairyLabel };

        var transactionWithPrimairyLabelRemoved = transactionWithPrimairyLabel with { PrimairyLabel = null };
        transactionWithPrimairyLabel.PrimairyLabel.ShouldBeNull();
        transactionWithPrimairyLabel.Labels.Length.ShouldBe(1);
        transactionWithPrimairyLabel.Labels[0].ShouldBe(primairyLabel);
    }

    [Fact]
    public void OverwritingPrimairyLabelShouldResultInBothValuesBeingLabels()
    {
        var primairyLabel = "This is such a superior label";
        var overwritingPrimairyLabel = "Who's the boss now?";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithPrimairyLabel = emptyTransaction with { PrimairyLabel = primairyLabel };

        var transactionWithPrimairyLabelOverwritten = transactionWithPrimairyLabel with { PrimairyLabel = overwritingPrimairyLabel };
        transactionWithPrimairyLabel.PrimairyLabel.ShouldBe(overwritingPrimairyLabel);
        transactionWithPrimairyLabel.Labels.Length.ShouldBe(2);
        transactionWithPrimairyLabel.Labels[0].ShouldBe(overwritingPrimairyLabel);
        transactionWithPrimairyLabel.Labels[1].ShouldBe(primairyLabel);
    }

    [Fact]
    public void PrimairyLabelShouldAlwaysBePresentInLabels()
    {
        var primairyLabel = "This is such a superior label";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithPrimairyLabel = emptyTransaction with { PrimairyLabel = primairyLabel };

        var transactionWhereAllLabelsAreRemoved = transactionWithPrimairyLabel with { Labels = Array.Empty<string>() };
        transactionWhereAllLabelsAreRemoved.ShouldBe(transactionWithPrimairyLabel);
        transactionWhereAllLabelsAreRemoved.PrimairyLabel.ShouldBe(primairyLabel);
        transactionWhereAllLabelsAreRemoved.Labels.Length.ShouldBe(1);
        transactionWhereAllLabelsAreRemoved.Labels[0].ShouldBe(primairyLabel);

        var transactionWherePrimairyLabelAndAllLabelsAreRemoved = transactionWithPrimairyLabel with
        {
            PrimairyLabel = null,
            Labels = Array.Empty<string>(),
        };
        transactionWherePrimairyLabelAndAllLabelsAreRemoved.PrimairyLabel.ShouldBeNull();
        transactionWherePrimairyLabelAndAllLabelsAreRemoved.Labels.ShouldBeEmpty();
    }

    [Fact]
    public void LabelsShouldNeverContainDuplicates()
    {
        var label = "I'm a basic label";
        var sameLabel = label;
        var otherLabel = "I'm not like other girls";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithMultipleUniqueLabels = emptyTransaction with { Labels = new[] { label, otherLabel } };
        transactionWithMultipleUniqueLabels.Labels.ShouldContain(label);
        transactionWithMultipleUniqueLabels.Labels.ShouldContain(otherLabel);
        transactionWithMultipleUniqueLabels.Labels.Length.ShouldBe(2);

        var transactionWithOnlyDuplicateLabels = emptyTransaction with { Labels = new[] { label, sameLabel } };
        transactionWithMultipleUniqueLabels.Labels.ShouldContain(label);
        transactionWithMultipleUniqueLabels.Labels.ShouldNotContain(otherLabel);
        transactionWithMultipleUniqueLabels.Labels.Length.ShouldBe(1);

        var transactionWithDuplicateAndOtherLabels = emptyTransaction with { Labels = new[] { label, sameLabel, otherLabel } };
        transactionWithMultipleUniqueLabels.Labels.ShouldContain(label);
        transactionWithMultipleUniqueLabels.Labels.ShouldContain(otherLabel);
        transactionWithMultipleUniqueLabels.Labels.Length.ShouldBe(2);
    }

    [Fact]
    public void IdentifierTests()
    {
        throw new NotImplementedException();
    }

    private Transaction GetEmptyTransaction() => new Transaction
    {
        Amount = 0,
        Currency = Currency.EUR,
        ThisPartyIdentifier = string.Empty,
        OtherPartyIdentifier = string.Empty,
        Timestamp = new DateTime(1970, 01, 01),
        RawData = string.Empty,
        Description = string.Empty,
        Labels = Array.Empty<string>(),
    };
}
