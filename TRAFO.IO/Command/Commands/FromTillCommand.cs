using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public abstract class FromTillCommand : Command
{
    protected FromTillCommand(string[] arguments, ICommandFlag[] flags) : base(arguments, flags)
    {
        _from = (FromFlag) flags.First(f => f is FromFlag);
        _till = (TillFlag) flags.First(f => f is TillFlag);
    }

    protected override sealed bool IsSupported(ICommandFlag flag) => flag is FromFlag or TillFlag;

    protected FromFlag _from { get; private init; }
    protected TillFlag _till { get; private init; }
}
