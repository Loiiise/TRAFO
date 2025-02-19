using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public interface ICommand
{
    public string[] Arguments { get; init; }
    public ICommandFlag[] Flags { get; init; }

    bool Validate([MaybeNullWhen(true), NotNullWhen(false)] out Exception exception);
    void Execute();
    bool TryExecute([MaybeNullWhen(true), NotNullWhen(false)] out Exception exception);
}
