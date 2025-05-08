using System.Diagnostics.CodeAnalysis;
using TRAFO.CLI.Command.MetaData;
using TRAFO.IO.Command;
using TRAFO.IO.Command.Flags;

namespace TRAFO.CLI.Command.Factory;

internal interface ICommandFlagStringFactory
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

internal class CommandFlagStringFactory : ICommandFlagStringFactory
{
    public CommandFlagStringFactory(IFlagMetaData flagMetaData, ICommandFlagFactory commandFlagFactory)
    {
        _flagMetaData = flagMetaData;
        _commandFlagFactory = commandFlagFactory;
    }

    public string FlagIndicator => "--";
    private readonly char _termSeparator = ' ';
    private readonly IFlagMetaData _flagMetaData;
    private readonly ICommandFlagFactory _commandFlagFactory;

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
        values = RemoveEmptyValuesAndWhiteSpaces(values);

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

        try
        {
            flag = _commandFlagFactory.GetFlag(flagName, values[1]);
        }
        catch (Exception ex)
        {
            exception = ex;
            return false;
        }

        return true;
    }


    private string[] RemoveEmptyValuesAndWhiteSpaces(string[] array)
        => array
            .Where(s => s != null && s != string.Empty && s != "")
            .Select(s => s.Trim())
            .ToArray();
}
