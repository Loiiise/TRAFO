using Shouldly;

namespace TRAFO.IO.Tests.Command;

public class CommandTests
{
    [Fact]
    public void CommandValidationFailsIfTheAmountOfArgumentsDoesntMatchExpected()
    {
        var twoArguments = new string[] { "I'm one argument", "No, you're wrong"};
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
}
