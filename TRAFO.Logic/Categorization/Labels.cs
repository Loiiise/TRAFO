using TRAFO.Logic.Categorization.Predicates;

namespace TRAFO.Logic.Categorization;

// todo #75: fix
public static class Labels
{
    public const string Groceries = nameof(Groceries);
    public const string Rent = nameof(Rent);


    public static TransactionPredicate[] GetDefaultPredicates() => new TransactionPredicate[]
    {
        // todo #55: these should not be hardcoded here
        new ContainsOtherPartyNamePredicate()
        {
            ContainedString = "AH TO GO",
            LabelToSet = nameof(Groceries),
        },
        new ContainsOtherPartyNamePredicate()
        {
            ContainedString = "ALBERT HEIJN",
            LabelToSet = nameof(Groceries),
        },
        new ContainsOtherPartyNamePredicate()
        {
            ContainedString = "JUMBO",
            LabelToSet = nameof(Groceries),
        },
    };
}
