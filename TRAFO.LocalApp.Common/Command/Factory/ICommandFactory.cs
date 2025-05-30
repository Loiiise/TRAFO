using System.Diagnostics.CodeAnalysis;
using TRAFO.LocalApp.Common.Command.Arguments;
using TRAFO.LocalApp.Common.Command.Flags;

namespace TRAFO.LocalApp.Common.Command;

public interface ICommandFactory
{
    ICommand GetCommand(string commandName, ICommandArgument[] args, ICommandFlag[] flags);
    bool TryGetCommand(string commandName, ICommandArgument[] args, ICommandFlag[] flags, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command);
}
