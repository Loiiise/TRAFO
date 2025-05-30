using System.Diagnostics.CodeAnalysis;
using TRAFO.LocalApp.Common.Command.Arguments;

namespace TRAFO.LocalApp.Common.Command;

public interface ICommandArgumentFactory
{
    ICommandArgument GetArgument(string commandName, string argumentValue);
    bool TryGetArgument(string commandName, string argumentValue, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandArgument argument);
}
