namespace TRAFO.IO.Tests.Command;
internal class MockDoNothingCommand : TRAFO.IO.Command.Command
{
    public MockDoNothingCommand(string[] arguments) : this(arguments, arguments.Length) { }

    public MockDoNothingCommand(string[] arguments, int expectedAmountOfArguments) : base(arguments)
    {
        _setableExpectedAmountOfArguments = expectedAmountOfArguments;
    }

    protected override int _expectedAmountOfArguments => _setableExpectedAmountOfArguments;

    public override void Execute() { }
    protected override void ValidateInternally() { }

    private int _setableExpectedAmountOfArguments;
}
