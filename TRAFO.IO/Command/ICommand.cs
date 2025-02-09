using System.Diagnostics.CodeAnalysis;

namespace TRAFO.IO.Command;

public interface ICommand
{
    public string Name { get; }
    public string[] Arguments { get; init; }

    bool Validate([MaybeNullWhen(true), NotNullWhen(false)] out Exception exception);
    void Execute();
    bool TryExecute([MaybeNullWhen(true), NotNullWhen(false)] out Exception exception);
}
