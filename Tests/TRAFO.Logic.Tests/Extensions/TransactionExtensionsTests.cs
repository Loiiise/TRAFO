using Shouldly;
using TRAFO.Logic.Extensions;
using static TRAFO.Logic.Tests.TransactionFixture;

namespace TRAFO.Logic.Tests.Extensions;
public class TransactionExtensionsTests
{

    [Fact]
    public void SetPrimairyLabelTest()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void SettingThePrimairyLabelShouldAlsoAddThatLabelToLabelsIfSpecified()
    {
        var primairyLabel = "This is such a superior label";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithPrimairyLabelInLabels = emptyTransaction.SetPrimairyLabel(primairyLabel, true);
        transactionWithPrimairyLabelInLabels.PrimairyLabel.ShouldBe(primairyLabel);
        transactionWithPrimairyLabelInLabels.Labels.Length.ShouldBe(1);
        transactionWithPrimairyLabelInLabels.Labels[0].ShouldBe(primairyLabel);

        var transactionWithPrimairyLabelNotInLabels = emptyTransaction.SetPrimairyLabel(primairyLabel, false);
        transactionWithPrimairyLabelNotInLabels.PrimairyLabel.ShouldBe(primairyLabel);
        transactionWithPrimairyLabelNotInLabels.Labels.Length.ShouldBe(0);
    }

    [Fact]
    public void OverwritingPrimairyLabelShouldResultInBothValuesBeingLabels()
    {
        var primairyLabel = "This is such a superior label";
        var overwritingPrimairyLabel = "Who's the boss now?";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithPrimairyLabel = emptyTransaction.SetPrimairyLabel(primairyLabel);

        var transactionWithPrimairyLabelOverwritten = transactionWithPrimairyLabel.SetPrimairyLabel(overwritingPrimairyLabel);
        transactionWithPrimairyLabel.PrimairyLabel.ShouldBe(overwritingPrimairyLabel);
        transactionWithPrimairyLabel.Labels.Length.ShouldBe(2);
        transactionWithPrimairyLabel.Labels[0].ShouldBe(overwritingPrimairyLabel);
        transactionWithPrimairyLabel.Labels[1].ShouldBe(primairyLabel);
    }

    [Fact]
    public void RemovePrimairyLabelTest()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void SettingPrimairyLabelToNullShouldNotRemoveItFromLabels()
    {
        var primairyLabel = "This is such a superior label";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithPrimairyLabel = emptyTransaction.SetPrimairyLabel(primairyLabel);

        var transactionWithPrimairyLabelRemoved = transactionWithPrimairyLabel.RemovePrimairyLabel();
        transactionWithPrimairyLabel.PrimairyLabel.ShouldBeNull();
        transactionWithPrimairyLabel.Labels.Length.ShouldBe(1);
        transactionWithPrimairyLabel.Labels[0].ShouldBe(primairyLabel);
    }

    [Fact]
    public void AddLabelTest()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void AddingALabelToLabelsShouldNotMakeItPrimairyLabelIfPrimairyLabelHasNotBeenSet()
    {
        var label = "This is a label";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithLabelInLabels = emptyTransaction.AddLabel(label);
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

        var transactionWithPrimairyLabel = emptyTransaction.SetPrimairyLabel(primairyLabel);
        transactionWithPrimairyLabel.PrimairyLabel.ShouldBe(primairyLabel);
        transactionWithPrimairyLabel.Labels.Length.ShouldBe(1);
        transactionWithPrimairyLabel.Labels[0].ShouldBe(primairyLabel);

        var transactionWithPrimairyLabelAndLabels = transactionWithPrimairyLabel.AddLabel(label);
        transactionWithPrimairyLabelAndLabels.PrimairyLabel.ShouldBe(primairyLabel);
        transactionWithPrimairyLabelAndLabels.Labels.Length.ShouldBe(2);
        transactionWithPrimairyLabelAndLabels.Labels[0].ShouldBe(primairyLabel);
        transactionWithPrimairyLabelAndLabels.Labels[1].ShouldBe(label);
    }


    [Fact]
    public void LabelsShouldNeverContainDuplicates()
    {
        throw new NotSupportedException();

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
    public void RemoveLabelTest()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void RemoveAllLabels()
    {
        throw new NotImplementedException();
    }


    [Fact]
    public void PrimairyLabelShouldAlwaysBePresentInLabels()
    {
        var primairyLabel = "This is such a superior label";

        var emptyTransaction = GetEmptyTransaction();
        emptyTransaction.PrimairyLabel.ShouldBeNull();
        emptyTransaction.Labels.ShouldBeEmpty();

        var transactionWithPrimairyLabel = emptyTransaction.SetPrimairyLabel(primairyLabel);

        var transactionWhereAllLabelsAreRemoved = transactionWithPrimairyLabel.RemoveAllLabels(true);
        transactionWhereAllLabelsAreRemoved.ShouldBe(transactionWithPrimairyLabel);
        transactionWhereAllLabelsAreRemoved.PrimairyLabel.ShouldBe(primairyLabel);
        transactionWhereAllLabelsAreRemoved.Labels.Length.ShouldBe(1);
        transactionWhereAllLabelsAreRemoved.Labels[0].ShouldBe(primairyLabel);

        var transactionWherePrimairyLabelAndAllLabelsAreRemoved = transactionWithPrimairyLabel.RemoveAllLabels(false);
        transactionWherePrimairyLabelAndAllLabelsAreRemoved.PrimairyLabel.ShouldBeNull();
        transactionWherePrimairyLabelAndAllLabelsAreRemoved.Labels.ShouldBeEmpty();
    }
}
