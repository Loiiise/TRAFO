namespace TRAFO.CLI;
internal class UserCommandHandler
{
    internal UserCommandHandler(ICommandParser commandParser, ICLIUserInputHandler userInputHandler)
    {
        _commandParser = commandParser;
        _userInputHandler = userInputHandler;
    }

    public void ExecuteUserCommand(string[] arguments)
    {
        if (_commandParser.TryParse(arguments, out var command))
        {
            ExecuteUserCommand(command);
            return;
        }

        _userInputHandler.TerminateTask($"Failed to parse \"{string.Join(' ', arguments)}\"");
    }

    public void ExecuteUserCommand(ICommand command)
    {
        _userInputHandler.PromptScopeUp(command.Name);

        if (command.Validate() &&
            command.Execute())
        {
            _userInputHandler.PromptScopeDown();
        }
    }

    private readonly ICommandParser _commandParser;
    private readonly ICLIUserInputHandler _userInputHandler;
}
