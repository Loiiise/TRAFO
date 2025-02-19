using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Tests.Command;

internal class MockDoNothingCommand : TRAFO.IO.Command.Command
{
    public MockDoNothingCommand(ICommandFlag[] flags, Func<ICommandFlag, bool> isSupported) : this(Array.Empty<string>(), 0, flags, isSupported) { }
    public MockDoNothingCommand(string[] arguments) : this(arguments, arguments.Length, Array.Empty<ICommandFlag>()) { }
    public MockDoNothingCommand(string[] arguments, int expectedAmountOfArguments) : this(arguments, expectedAmountOfArguments, Array.Empty<ICommandFlag>()) { }
    public MockDoNothingCommand(string[] arguments, int expectedAmountOfArguments, ICommandFlag[] flags) : this(arguments, expectedAmountOfArguments, flags, _ => true) { }
    public MockDoNothingCommand(string[] arguments, int expectedAmountOfArguments, ICommandFlag[] flags, Func<ICommandFlag, bool> isSupported) : base(arguments, flags)
    {
        _setableExpectedAmountOfArguments = expectedAmountOfArguments;
        _isSupported = isSupported;
    }

    protected override int _expectedAmountOfArguments => _setableExpectedAmountOfArguments;

    public override void Execute() { }
    protected override void ValidateInternally() { }

    protected override bool IsSupported(ICommandFlag flag) => _isSupported(flag);

    private int _setableExpectedAmountOfArguments;
    private readonly Func<ICommandFlag, bool> _isSupported;
}
