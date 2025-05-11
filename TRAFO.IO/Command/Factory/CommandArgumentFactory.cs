using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Arguments;

namespace TRAFO.IO.Command.Factory;
public class CommandArgumentFactory : ICommandArgumentFactory
{
    public ICommandArgument GetArgument(string commandName, string argumentValue)
        => GetArgumentSafe(commandName, argumentValue, out var argument, out var exception) ?
            argument :
            throw exception;

    public bool TryGetArgument(string commandName, string argumentValue, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandArgument argument)
        => GetArgumentSafe(commandName, argumentValue, out argument, out _);

    public bool GetArgumentSafe(string commandName, string argumentValue, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandArgument argument, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        if (commandName == nameof(LoadTransactionFileCommand))
        {
            argument = new FilePathArgument
            {
                Value = argumentValue,
            };
            exception = null;
            return true;
        }

        argument = null;
        exception = new NotSupportedException($"No valid argument could be parsed for {commandName} with value {argumentValue}");
        return false;
    }
}
