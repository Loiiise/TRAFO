using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public interface ICommandFlagFactory
{
    ICommandFlag GetFlag(string flagName, string flagValue);
    bool TryGetFlag(string flagName, string flagValue, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag);
}
