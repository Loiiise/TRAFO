namespace TRAFO.IO.Command;

public class CommandFactory : ICommandFactory
{
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

    private bool FromArgumentsSafe(string[] arguments, out ICommand command, out Exception exception)
    {
        throw new NotImplementedException();
    }
}
