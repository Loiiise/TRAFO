using System.Diagnostics.CodeAnalysis;

namespace TRAFO.IO.Command;

public class CommandFactory : ICommandFactory
{
    private readonly IBasicUserOutputHandler _userOutputHandler;

    public CommandFactory(IBasicUserOutputHandler userOutputHandler)
    {
        _userOutputHandler = userOutputHandler;
    }

    public ICommand FromString(string input) => FromArguments(input.Split(' ').ToArray());

    public ICommand FromArguments(string[] arguments)
        => FromArgumentsSafe(arguments, out var command, out var exception)
        ? command
        : throw exception;

    public ICommand FromCommandNameAndArguments(string commandName, string[] arguments)
        => FromArguments(arguments.Prepend(commandName).ToArray());

    public bool TryFromString(string input, out ICommand command) => TryFromArguments(input.Split(' ').ToArray(), out command);

    public bool TryFromArguments(string[] arguments, out ICommand command)
        => FromArgumentsSafe(arguments, out command, out var _);

    public bool TryFromCommandNameAndArguments(string commandName, string[] arguments, out ICommand command)
        => TryFromArguments(arguments.Prepend(commandName).ToArray(), out command);

    protected bool FromArgumentsSafe(string[] arguments, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        if (!arguments.Any())
        {
            command = null;
            exception = new ArgumentException("No command was provided");
            return false;
        }

        command = GetCommand(arguments[0]);
        exception = null;
        return true;
    }

    private ICommand GetCommand(string commandName) => commandName switch
    {
        nameof(HelpCommand) => new HelpCommand(_userOutputHandler, this),
        _ => throw new NotImplementedException(),
    };

    public IEnumerable<string> AllCommandNames()
    {
        throw new NotImplementedException();
    }


    public string GetTag(ICommand command) => GetTag(nameof(command));
    public string GetTag(string commandName)
    {
        throw new NotImplementedException();
    }

    public string GetDescription(ICommand command) => GetDescription(nameof(command));
    public string GetDescription(string commandName)
    {
        throw new NotImplementedException();
    }
}
