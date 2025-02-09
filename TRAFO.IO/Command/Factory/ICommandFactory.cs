using System.Diagnostics.CodeAnalysis;

namespace TRAFO.IO.Command;

public interface ICommandFactory
{
    ICommand FromString(string input);
    ICommand FromArguments(string[] arguments);
    ICommand FromCommandNameAndArguments(string commandName, string[] arguments);

    bool TryFromString(string input, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command);
    bool TryFromArguments(string[] arguments, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command);
    bool TryFromCommandNameAndArguments(string commandName, string[] arguments, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command);
}
