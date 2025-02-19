using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public class CommandFlagFactory : ICommandFlagFactory
{
    public CommandFlagFactory(IFlagMetaData flagMetaData)
    {
        _flagMetaData = flagMetaData;
    }

    public string FlagIndicator => "--";
    private readonly char _termSeparator = ' ';
    private readonly IFlagMetaData _flagMetaData;

    public ICommandFlag FromString(string value) => FromStrings(value.Split(_termSeparator));
    public ICommandFlag FromStrings(string[] values) => FromStringsSafe(values, out var flag, out var exception) ? flag : throw exception;

    public bool TryFromString(string value, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag) => TryFromStrings(value.Split(_termSeparator), out flag);
    public bool TryFromStrings(string[] values, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag) => FromStringsSafe(values, out flag, out _);

    public ICommandFlag[] AllFromString(string value) => AllFromStrings(value.Split(_termSeparator));
    public ICommandFlag[] AllFromStrings(string[] values) => AllFromStringsSafe(values, out var flags, out var exception) ? flags : throw exception;

    public bool TryAllFromString(string value, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag[] flags) => TryAllFromStrings(value.Split(_termSeparator), out flags);
    public bool TryAllFromStrings(string[] values, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag[] flags) => AllFromStringsSafe(values, out flags, out _);

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

    private bool FromStringsSafe(string[] values, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandFlag flag, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        flag = default;
        exception = null;
        if (values.Length != 2)
        {
            exception = new ArgumentException("Flags should have an indicator and a value");
            return false;
        }
        if (!values[0].StartsWith(FlagIndicator))
        {
            exception = new ArgumentException($"All flags should be indicated with {FlagIndicator}");
            return false;
        }

        var flagName = _flagMetaData.GetNameFromTag(values[0].Substring(FlagIndicator.Length));

        if (flagName == nameof(FromFlag) &&
            ParseDateTimeSafe(values[1], out var fromFlagDateTime, out exception))
        {
            flag = new FromFlag { Value = fromFlagDateTime };
        }
        else if (flagName == nameof(TillFlag) &&
            ParseDateTimeSafe(values[1], out var tillFlagDateTime, out exception))
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
