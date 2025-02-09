namespace TRAFO.IO.Command;

internal class CommandFactory : ICommandFactory
{
    public ICommand FromString(string input) => FromArguments(new string[] { input });

    public ICommand FromArguments(string[] arguments)
        => FromArgumentsSafe(arguments, out var command, out var exception)
        ? command
        : throw exception;

    public bool TryFromsString(string input, out ICommand command) => TryFromArguments(new string[] { input }, out command);

    public bool TryFromArguments(string[] arguments, out ICommand command)
        => FromArgumentsSafe(arguments, out command, out var _);

    private bool FromArgumentsSafe(string[] arguments, out ICommand command, out Exception exception)
    {
        throw new NotImplementedException();
    }
}
