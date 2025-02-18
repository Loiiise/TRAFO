using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public abstract class NoArgumentAndNoFlagCommand : Command
{
    public NoArgumentAndNoFlagCommand() : base(Array.Empty<string>(), Array.Empty<ICommandFlag>()) { }

    protected override sealed void ValidateInternally() { }
    public abstract override void Execute();

    protected override bool IsSupported(ICommandFlag flag) => false;
    protected override sealed int _expectedAmountOfArguments => 0;
}
