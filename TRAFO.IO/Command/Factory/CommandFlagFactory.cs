using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public class CommandFlagFactory : ICommandFlagFactory
{
    public string FlagIndicator => "--";
    private readonly char _termSeparator = ' ';

    public ICommandFlag FromString(string value) => FromStrings(value.Split(_termSeparator));
    public ICommandFlag FromStrings(string[] values) => FromStringsSafe(values, out var flag, out var exception) ? flag : throw exception;

    public bool TryFromString(string value, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag) => TryFromStrings(value.Split(_termSeparator), out flag);
    public bool TryFromStrings(string[] values, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag) => FromStringsSafe(values, out flag, out _);

    public ICommandFlag[] AllFromString(string value) => AllFromStrings(value.Split(_termSeparator));
    public ICommandFlag[] AllFromStrings(string[] values) => AllFromStringsSafe(values, out var flags, out var exception) ? flags : throw exception;

    public bool TryAllFromString(string value, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag[] flags) => TryAllFromStrings(value.Split(_termSeparator), out flags);
    public bool TryAllFromStrings(string[] values, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag[] flags) => AllFromStringsSafe(values, out flags, out _);

    private bool FromStringsSafe(string[] values, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        throw new NotImplementedException();
    }

    private bool AllFromStringsSafe(string[] values, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag[] flags, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        if (!values.Any())
        {
            exception = null;
            flags = Array.Empty<ICommandFlag>();
            return true;
        }

        var flagsList = new List<ICommandFlag>();
        int startCurrentFlag = 0;

        for (int i = startCurrentFlag + 1; i < values.Length; i++)
        {
            if (!values[i].StartsWith(FlagIndicator)) continue;

            if (!FromStringsSafe(values[startCurrentFlag..i], out var flag, out exception))
            {
                flags = null;
                return false;
            }

            flagsList.Add(flag);
            startCurrentFlag = i;
        }

        if (!FromStringsSafe(values[startCurrentFlag..], out var lastFlag, out exception))
        {
            flags = null;
            return false;
        }

        flags = flagsList.Append(lastFlag).ToArray();
        exception = null;
        return true;
    }
}
