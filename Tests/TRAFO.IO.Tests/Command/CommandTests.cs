using Shouldly;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Tests.Command;

public class CommandTests
{
    [Fact]
    public void CommandValidationFailsIfTheAmountOfArgumentsDoesntMatchExpected()
    {
        var twoArguments = new string[] { "I'm one argument", "No, you're wrong" };
        int expectedArguments = 2;

        new MockDoNothingCommand(twoArguments, expectedArguments).Validate(out var notAnException).ShouldBeTrue();
        notAnException.ShouldBeNull();

        // Oh no, three is not two
        expectedArguments = 3;
        new MockDoNothingCommand(twoArguments, expectedArguments).Validate(out var exception).ShouldBeFalse();
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        exception.Message.ShouldContain(expectedArguments.ToString());
        exception.Message.ShouldContain(twoArguments.Length.ToString());
    }

    [Fact]
    public void CommandValidationFailsIfAnUnsupportedFlagIsProvided()
    {
        Func<ICommandFlag, bool> acceptOnlyFromFlag = c => c is FromFlag;
        ICommandFlag[]
            emptyFlags = Array.Empty<ICommandFlag>(),
            fromFlags = { new FromFlag { Value = DateTime.MinValue } },
            otherFlags = { new TillFlag { Value = DateTime.MinValue } };

        new MockDoNothingCommand(emptyFlags, acceptOnlyFromFlag).Validate(out var notAndExceptionForEmptyFlags).ShouldBeTrue();
        notAndExceptionForEmptyFlags.ShouldBeNull();

        new MockDoNothingCommand(fromFlags, acceptOnlyFromFlag).Validate(out var notAndExceptionForCorrectFlags).ShouldBeTrue();
        notAndExceptionForCorrectFlags.ShouldBeNull();

        new MockDoNothingCommand(otherFlags, acceptOnlyFromFlag).Validate(out var exception).ShouldBeFalse();
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
    }
}
