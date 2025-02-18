using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public abstract class NoFlagCommand : Command
{
    public NoFlagCommand(string[] arguments) : base(arguments, Array.Empty<ICommandFlag>()) { }

    protected override bool IsSupported(ICommandFlag flag) => false;
}
