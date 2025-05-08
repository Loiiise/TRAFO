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

    protected override sealed bool IsSupported(ICommandFlag flag) => flag is FromFlag or TillFlag || AdditionalSupported();
    /// <summary>
    /// If an implementation of <see cref="FromTillCommand"/> has supports other flags too, they can add them here.
    /// </summary>
    protected virtual bool AdditionalSupported() => false;

    protected DateTime? _from { get; private init; }
    protected DateTime? _till { get; private init; }
}
