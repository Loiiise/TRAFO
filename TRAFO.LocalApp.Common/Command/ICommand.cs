using System.Diagnostics.CodeAnalysis;
using TRAFO.LocalApp.Common.Command.Flags;

namespace TRAFO.LocalApp.Common.Command;

public interface ICommand
{
    public ICommandFlag[] Flags { get; init; }

    void Execute();
    bool TryExecute([MaybeNullWhen(true), NotNullWhen(false)] out Exception exception);
}
