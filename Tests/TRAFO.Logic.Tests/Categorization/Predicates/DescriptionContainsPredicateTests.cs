using Shouldly;
using TRAFO.Logic.Categorization.Predicates;

namespace TRAFO.Logic.Tests.Categorization.Predicates;

public class DescriptionContainsPredicateTests
{
    [Theory, MemberData(nameof(GenerateExampleObjects))]
    public void DescriptionContainsPredicateReturnsTrueIsStringIsSubstring(string transactionDescription, string testString, bool shouldBeInThere)
    {
        var predicate = new ContainsDescriptionPredicate() 
        { 
            LabelToSet = "I am a label",
            ContainedString = testString 
        };
        var transaction = TransactionFixture.GetEmptyTransaction() with { Description = transactionDescription };

        predicate.IsValid(transaction).ShouldBe(shouldBeInThere);
    }

    [Theory, MemberData(nameof(GenerateExampleObjects))]
    public void NullTransactionDescriptionAlwaysReturnsFalse(string _0, string testString, bool _1)
    {
        var predicate = new ContainsDescriptionPredicate()
        {
            LabelToSet = "I am a label",
            ContainedString = testString
        };
        var transaction = TransactionFixture.GetEmptyTransaction() with { Description = null };

        predicate.IsValid(transaction).ShouldBeFalse();
    }

    [Fact]
    public void CaseSensitivityOnlyMattersWhenTheFlagIsSet()
    {
        var aString = "blablabla";
        var aSubstring = "bla";
        var aSubstringWithDifferentCapitalization = "bLA";

        CheckConfiguration(aString, aSubstring, false, true);
        CheckConfiguration(aString, aSubstring, true, true);

        CheckConfiguration(aString, aSubstringWithDifferentCapitalization, false, true);
        CheckConfiguration(aString, aSubstringWithDifferentCapitalization, true, false);

        void CheckConfiguration(string transactionDescription, string testString, bool predicateIsCaseSensitive, bool shouldBeInThere)
        {
            var predicate = new ContainsDescriptionPredicate()
            {
                LabelToSet = "I am a label",
                ContainedString = testString,
                CaseSensitive = predicateIsCaseSensitive,
            };
            var transaction = TransactionFixture.GetEmptyTransaction() with { Description = transactionDescription };

            predicate.IsValid(transaction).ShouldBe(shouldBeInThere);
        }
    }

    public static IEnumerable<object[]> GenerateExampleObjects() 
        => GenerateExamples().Select(example => new object[] { example.transactionDescription, example.testString, example.contains });  

    public static IEnumerable<(string transactionDescription, string testString, bool contains)> GenerateExamples() => new (string, string, bool)[]
    {
        ("This is a sentence", "This is not a substring of that sentence", false),
        ("AAAAAAAAAAAAAAAAAAAAAAAAAAAAA", "AAAAA", true),
        ("blablabla", "bla", true),
        (string.Empty, "something", false),
    };
}
