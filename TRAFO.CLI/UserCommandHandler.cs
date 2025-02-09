using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TRAFO.IO.Command;

namespace TRAFO.CLI;
internal class UserCommandHandler : BackgroundService
{
    public UserCommandHandler(
        ICommandFactory commandFactory,
        IBasicUserInputHandler basicUserInputHandler,
        IUserCommunicationHandler userCommunicationHandler,
        string[] initialArguments)
    {
        _initialArguments = initialArguments;
        _commandFactory = commandFactory;
        _basicUserInputHandler = basicUserInputHandler;
        _userCommunicationHandler = userCommunicationHandler;
    }

    private void ExecuteUserCommand(string line) => ExecuteUserCommand(
        () => (_commandFactory.TryFromString(line, out var command), command),
        line);
    private void ExecuteUserCommand(string[] arguments) => ExecuteUserCommand(
        () => (_commandFactory.TryFromArguments(arguments, out var command), command),
        string.Join(' ', arguments));

    private void ExecuteUserCommand(Func<(bool, ICommand)> tryGetCommand, string stringfiedRawObject)
    {
        _userCommunicationHandler.PromptScopeUp("CommandGenerating");

        (var commandReceived, var command) = tryGetCommand();
        if (commandReceived)
        {
            _userCommunicationHandler.PromptScopeDown();
            ExecuteUserCommand(command);
            return;
        }

        _userCommunicationHandler.TerminateTask($"Failed to parse \"{stringfiedRawObject}\"");
    }

    private void ExecuteUserCommand(ICommand command)
    {
        _userCommunicationHandler.PromptScopeUp(command.Name);

        if (!command.Validate(out var validationException))
        {
            _userCommunicationHandler.TerminateTask($"Command validation failed: {validationException.Message}");
            return;
        }

        if (!command.TryExecute(out var executionException))
        {
            _userCommunicationHandler.TerminateTask($"Command execution failed: {executionException.Message}");
        }

        _userCommunicationHandler.PromptScopeDown();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) => await Task.Run(() =>
    {
        if (_initialArguments.Any())
        {
            ExecuteUserCommand(_initialArguments);
        }
        else
        {
            var userInput = _basicUserInputHandler.GetUserInput("What do you want to do? Use the \"help\" command if you're not sure!");
            ExecuteUserCommand(userInput);
        }
    });

    private readonly string[] _initialArguments;
    private readonly ICommandFactory _commandFactory;
    private readonly IBasicUserInputHandler _basicUserInputHandler;
    private readonly IUserCommunicationHandler _userCommunicationHandler;
}
