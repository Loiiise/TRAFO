using TRAFO.Logic.Categorization.Predicates;

namespace TRAFO.Logic.Categorization;

public static class Categories
{
    public const string Groceries = nameof(Groceries);
    public const string Rent = nameof(Rent);

    public static TransactionPredicate[] GetDefaultPredicates() => new TransactionPredicate[]
    {
        // todo #55: these should not be hardcoded here
        new DescriptionContainsPredicate()
        {
            ContainedString = "AH TO GO",
            LabelToSet = nameof(Groceries),
        },
        new DescriptionContainsPredicate()
        {
            ContainedString = "ALBERT HEIJN",
            LabelToSet = nameof(Groceries),
        },
        new DescriptionContainsPredicate()
        {
            ContainedString = "JUMBO",
            LabelToSet = nameof(Groceries),
        },
    };
}
