using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Arguments;

namespace TRAFO.IO.Command;

public interface ICommandArgumentFactory
{
    ICommandArgument GetArgument(string commandName, string argumentValue);
    bool TryGetArgument(string commandName, string argumentValue, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandArgument argument);
}
