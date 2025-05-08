using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Arguments;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public interface ICommandFactory
{
    ICommand GetCommand(string commandName, ICommandArgument[] args, ICommandFlag[] flags);
    bool TryGetCommand(string commandName, ICommandArgument[] args, ICommandFlag[] flags, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command);
}
