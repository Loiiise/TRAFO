using System.Diagnostics.CodeAnalysis;
using TRAFO.CLI.Command.MetaData;
using TRAFO.IO.Command;
using TRAFO.IO.Command.Arguments;

namespace TRAFO.CLI.Command.Factory;

public interface ICommandStringFactory
{
    ICommand FromString(string input);
    ICommand FromArguments(string[] arguments);
    ICommand FromCommandNameAndArguments(string commandName, string[] arguments);

    bool TryFromString(string input, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command);
    bool TryFromArguments(string[] arguments, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command);
    bool TryFromCommandNameAndArguments(string commandName, string[] arguments, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command);
}

internal class CommandStringFactory
{
    public CommandStringFactory(
        ICommandFactory commandFactory,
        ICommandArgumentFactory commandArgumentFactory,
        ICommandFlagStringFactory commandFlagStringFactory,
        ICommandMetaData commandMetaData)
    {
        _commandFactory = commandFactory;
        _commandArgumentFactory = commandArgumentFactory;
        _commandFlagStringFactory = commandFlagStringFactory;
        _commandMetaData = commandMetaData;
    }

    internal ICommand FromString(string input) => FromArguments(input.Split(' ').ToArray());

    internal ICommand FromArguments(string[] arguments)
        => FromArgumentsSafe(arguments, out var command, out var exception)
        ? command
        : throw exception;

    internal ICommand FromCommandNameAndArguments(string commandName, string[] arguments)
        => FromArguments(arguments.Prepend(commandName).ToArray());

    internal bool TryFromString(string input, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command) => TryFromArguments(input.Split(' ').ToArray(), out command);

    internal bool TryFromArguments(string[] arguments, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command)
        => FromArgumentsSafe(arguments, out command, out var _);

    internal bool TryFromCommandNameAndArguments(string commandName, string[] arguments, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command)
        => TryFromArguments(arguments.Prepend(commandName).ToArray(), out command);

    protected bool FromArgumentsSafe(string[] arguments, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        command = default;
        exception = null;

        if (!arguments.Any())
        {
            exception = new ArgumentException("No command was provided");
            return false;
        }

        var commandName = _commandMetaData.GetNameFromTag(arguments[0]);

        try
        {
            int i = 1;

            List<ICommandArgument> commandArguments = new();
            while (i < arguments.Length && !arguments[i].StartsWith(_commandFlagStringFactory.FlagIndicator))
            {
                var commandArgument = _commandArgumentFactory.GetArgument(commandName, arguments[i++]);
                commandArguments.Add(commandArgument);
            }

            var flags = _commandFlagStringFactory.AllFromStrings(arguments[i..]);

            command = _commandFactory.GetCommand(commandName, commandArguments.ToArray(), flags);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        return exception == null;
    }

    private readonly ICommandFactory _commandFactory;
    private readonly ICommandArgumentFactory _commandArgumentFactory;
    private readonly ICommandFlagStringFactory _commandFlagStringFactory;

    private readonly ICommandMetaData _commandMetaData;
}
