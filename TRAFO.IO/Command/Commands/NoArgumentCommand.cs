using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public abstract class NoArgumentCommand : Command
{
    protected NoArgumentCommand(ICommandFlag[] flags) : base(Array.Empty<string>(), flags) { }

    protected override sealed int _expectedAmountOfArguments => 0;
    protected override void ValidateInternally() { }
}
