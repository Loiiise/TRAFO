using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

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
        else if (exception != null)
        {
            exception = new ArgumentException($"{flagName} is not a valid flag.");
        }

        return exception == null;
    }

    private bool ParseDateTimeSafe(string dateTimeString, [MaybeNullWhen(false), NotNullWhen(true)] out DateTime dateTime, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        var isDateTime = DateTime.TryParse(dateTimeString, out dateTime);

        exception = isDateTime ? null : new ArgumentException($"{dateTimeString} could not be parsed as a date.");
        return isDateTime;
    }
}
