using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Tests.Command;

internal class MockDoNothingCommand : TRAFO.IO.Command.Command
{
    public MockDoNothingCommand(string[] arguments) : this(arguments, arguments.Length, Array.Empty<ICommandFlag>()) { }
    public MockDoNothingCommand(string[] arguments, int expectedAmountOfArguments) : this(arguments, expectedAmountOfArguments, Array.Empty<ICommandFlag>()) { }
    public MockDoNothingCommand(string[] arguments, int expectedAmountOfArguments, ICommandFlag[] flags) : base(arguments, flags)
    {
        _setableExpectedAmountOfArguments = expectedAmountOfArguments;
    }

    protected override int _expectedAmountOfArguments => _setableExpectedAmountOfArguments;

    public override void Execute() { }
    protected override void ValidateInternally() { }

    protected override bool IsSupported(ICommandFlag flag) => true;

    private int _setableExpectedAmountOfArguments;
}
