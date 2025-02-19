using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public interface ICommandFlagFactory
{
    ICommandFlag FromString(string value);
    ICommandFlag FromStrings(string[] values);

    bool TryFromString(string value, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag);
    bool TryFromStrings(string[] values, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag);

    ICommandFlag[] AllFromString(string value);
    ICommandFlag[] AllFromStrings(string[] values);

    bool TryAllFromString(string value, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag[] flags);
    bool TryAllFromStrings(string[] values, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag[] flags);

    string FlagIndicator { get; }
}
