using System.Diagnostics.CodeAnalysis;
using TRAFO.LocalApp.Common.Command.Flags;

namespace TRAFO.LocalApp.Common.Command;

public interface ICommandFlagFactory
{
    ICommandFlag GetFlag(string flagName, string flagValue);
    bool TryGetFlag(string flagName, string flagValue, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag);
}
