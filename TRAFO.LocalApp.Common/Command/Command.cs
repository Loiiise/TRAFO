using System.Diagnostics.CodeAnalysis;
using TRAFO.LocalApp.Common.Command.Flags;

namespace TRAFO.LocalApp.Common.Command;

public abstract class Command : ICommand
{
    public ICommandFlag[] Flags { get; init; }

    protected Command(ICommandFlag[] flags)
    {
        Flags = flags;
    }

    public abstract void Execute();
    public bool TryExecute([MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        try
        {
            Execute();
        }
        catch (Exception internalException)
        {
            exception = internalException;
            return false;
        }

        exception = null;
        return true;
    }
}
