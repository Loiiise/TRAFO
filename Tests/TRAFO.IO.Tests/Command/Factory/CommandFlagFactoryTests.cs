using Shouldly;
using TRAFO.IO.Command;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Tests.Command;

public class CommandFlagFactoryTests
{
    [Fact]
    public void EmptyFlagStringShouldReturnEmptyCollection()
    {
        var factory = GetFactory();

        var flags = factory.AllFromString("");
        flags.ShouldBeEmpty();
    }

    [Theory, MemberData(nameof(GenerateDateTimes))]
    public void SingleFlagShouldReturnSingleItem(DateTime dateTime)
    {
        var factory = GetFactory();

        var flags = factory.AllFromString($"--from {dateTime.ToString("yyyy-MM-dd")}");

        flags.Count().ShouldBe(1);
        var fromFlag = flags[0].ShouldBeOfType<FromFlag>();
        fromFlag.Value.Year.ShouldBe(dateTime.Year);
        fromFlag.Value.Month.ShouldBe(dateTime.Month);
        fromFlag.Value.Day.ShouldBe(dateTime.Day);
    }

    [Theory, CombinatorialData]
    public void MultipleFlagsShouldReturnMultipleItems(
        [CombinatorialMemberData(nameof(GenerateDateTimes))] DateTime fromDateTime,
        [CombinatorialMemberData(nameof(GenerateDateTimes))] DateTime tillDateTime)
    {
        var factory = GetFactory();

        var flags = factory.AllFromString($"--from {fromDateTime.ToString("yyyy-MM-dd")} --till {tillDateTime.ToString("yyyy-MM-dd")}");

        flags.Count().ShouldBe(2);

        var fromFlag = flags[0].ShouldBeOfType<FromFlag>();
        fromFlag.Value.Year.ShouldBe(fromDateTime.Year);
        fromFlag.Value.Month.ShouldBe(fromDateTime.Month);
        fromFlag.Value.Day.ShouldBe(fromDateTime.Day);

        var tillFlag = flags[1].ShouldBeOfType<TillFlag>();
        tillFlag.Value.Year.ShouldBe(tillDateTime.Year);
        tillFlag.Value.Month.ShouldBe(tillDateTime.Month);
        tillFlag.Value.Day.ShouldBe(tillDateTime.Day);
    }

    private ICommandFlagFactory GetFactory() => new CommandFlagFactory(new FlagMetaData());

    public static IEnumerable<object[]> GenerateDateTimes()
        => new DateTime[]
        {
            new DateTime(1912,06,23),
            new DateTime(1930,05,11),
            new DateTime(2000,09,06),
        }
        .Select(x => new object[] { x });
}
