using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public interface ICommand
{
    public ICommandFlag[] Flags { get; init; }

    void Execute();
    bool TryExecute([MaybeNullWhen(true), NotNullWhen(false)] out Exception exception);
}
