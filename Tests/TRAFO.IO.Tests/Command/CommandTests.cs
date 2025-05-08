using Shouldly;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Tests.Command;

public class CommandTests
{

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
