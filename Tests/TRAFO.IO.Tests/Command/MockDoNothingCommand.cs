using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Tests.Command;

internal class MockDoNothingCommand : TRAFO.IO.Command.Command
{
    public MockDoNothingCommand(ICommandFlag[] flags, Func<ICommandFlag, bool> isSupported) : this(Array.Empty<string>(), flags, isSupported) { }
    public MockDoNothingCommand(string[] arguments) : this(arguments, Array.Empty<ICommandFlag>()) { }
    public MockDoNothingCommand(string[] arguments, ICommandFlag[] flags) : this(arguments, flags, _ => true) { }
    public MockDoNothingCommand(string[] arguments, ICommandFlag[] flags, Func<ICommandFlag, bool> isSupported) : base(flags)
    {
        _isSupported = isSupported;
    }

    public override void Execute() { }
    protected override void ValidateInternally() { }

    protected override bool IsSupported(ICommandFlag flag) => _isSupported(flag);

    private int _setableExpectedAmountOfArguments;
    private readonly Func<ICommandFlag, bool> _isSupported;
}
