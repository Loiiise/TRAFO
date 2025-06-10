using System.Diagnostics.CodeAnalysis;
using TRAFO.LocalApp.Common.Command.Flags;

namespace TRAFO.LocalApp.Common.Command;

public class CommandFlagFactory : ICommandFlagFactory
{
    public ICommandFlag GetFlag(string flagName, string flagValue)
        => FromStringSafe(flagName, flagValue, out var flag, out var exception) ?
            flag :
            throw exception;

    public bool TryGetFlag(string flagName, string flagValue, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag)
        => FromStringSafe(flagName, flagValue, out flag, out var _);

    public bool FromStringSafe(string flagName, string flagValue, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        flag = default;
        exception = null;

        if (flagName == nameof(FromFlag) &&
            ParseDateTimeSafe(flagValue, out var fromFlagDateTime, out exception))
        {
            flag = new FromFlag { Value = fromFlagDateTime };
        }
        else if (flagName == nameof(TillFlag) &&
            ParseDateTimeSafe(flagValue, out var tillFlagDateTime, out exception))
        {
            flag = new TillFlag { Value = tillFlagDateTime };
        }
        else if (flagName == nameof(DateFlag) &&
            ParseDateTimeSafe(flagValue, out var dateTime, out exception))
        {
            flag = new DateFlag { Value = dateTime };
        }
        else if (flagName == nameof(SkipFirstLineFlag) &&
            ParseBoolSafe(flagValue, out var skipFirstLine, out exception))
        {
            flag = new SkipFirstLineFlag { Value = skipFirstLine };
        }
        else if (exception != null)
        {
            exception = new ArgumentException($"{flagName} is not a valid flag.");
        }

        return exception == null;
    }

    private bool ParseBoolSafe(string dateTimeString, [MaybeNullWhen(false), NotNullWhen(true)] out bool boolean, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
        => ParseXSafe(dateTimeString, bool.TryParse, out boolean, out exception);

    private bool ParseDateTimeSafe(string dateTimeString, [MaybeNullWhen(false), NotNullWhen(true)] out DateTime dateTime, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
        => ParseXSafe(dateTimeString, DateTime.TryParse, out dateTime, out exception);

    private bool ParseXSafe<T>(
        string inputString,
        TryParseDelegate<T> parseDelegate,
        [MaybeNullWhen(false), NotNullWhen(true)] out T? item,
        [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)

    {
        var isX = parseDelegate(inputString, out item);

        exception = isX ? null : new ArgumentException($"{inputString} could not be parsed as a {typeof(T)}.");
        return isX;
    }
    public delegate bool TryParseDelegate<T>(string input, out T result);
}
