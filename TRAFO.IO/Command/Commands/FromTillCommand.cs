using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public abstract class FromTillCommand : Command
{
    protected FromTillCommand(ICommandFlag[] flags) : base(flags)
    {
        var fromFlags = flags.Where(f => f is FromFlag).Cast<FromFlag>();
        var tillFlags = flags.Where(f => f is TillFlag).Cast<TillFlag>();

        _from = fromFlags.Any() ? fromFlags.First().Value : null;
        _till = tillFlags.Any() ? tillFlags.First().Value : null;
    }

    protected DateTime? _from { get; private init; }
    protected DateTime? _till { get; private init; }
}
