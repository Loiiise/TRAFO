using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public abstract class Command : ICommand
{
    public ICommandFlag[] Flags { get; init; }

    protected Command(ICommandFlag[] flags)
    {
        Flags = flags;
    }

    public bool Validate([MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        foreach (var flag in Flags)
        {
            if (!IsSupported(flag))
            {
                exception = new ArgumentException($"{flag.GetType()} is not supported for {this.GetType()}");
                return false;
            }
        }

        try
        {
            ValidateInternally();
        }
        catch (Exception internalException)
        {
            exception = internalException;
            return false;
        }

        exception = null;
        return true;
    }
    protected abstract void ValidateInternally();
    protected abstract bool IsSupported(ICommandFlag flag);

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
