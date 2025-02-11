using System.Diagnostics.CodeAnalysis;

namespace TRAFO.IO.Command;

public abstract class Command : ICommand
{
    public string[] Arguments { get; init; }

    protected Command(string[] arguments)
    {
        Arguments = arguments;
    }

    public bool Validate([MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        if (Arguments.Length != _expectedAmountOfArguments)
        {
            exception = new ArgumentException($"Expected {_expectedAmountOfArguments} arguments, received {Arguments.Length}");
            return false;
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

    protected abstract int _expectedAmountOfArguments { get; }
}
