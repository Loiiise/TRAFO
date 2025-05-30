using TRAFO.LocalApp.Common.Command.Flags;
using TRAFO.Logic.Extensions;

namespace TRAFO.LocalApp.Common.Command;

public abstract class FromTillCommand : Command
{
    protected FromTillCommand(ICommandFlag[] flags) : base(flags)
    {
        FromFlag = flags.GetFirstOrDefault<FromFlag>();
        TillFlag = flags.GetFirstOrDefault<TillFlag>();
    }

    protected FromFlag? FromFlag { get; private init; }
    protected TillFlag? TillFlag { get; private init; }

    protected DateTime? _from { get => FromFlag == null ? null : FromFlag.Value; }
    protected DateTime? _till { get => TillFlag == null ? null : TillFlag.Value; }
}
