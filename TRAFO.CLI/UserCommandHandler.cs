using Microsoft.Extensions.Hosting;
using TRAFO.CLI.Command;
using TRAFO.CLI.Command.Factory;
using TRAFO.CLI.Command.MetaData;
using TRAFO.IO.Command;

namespace TRAFO.CLI;
internal class UserCommandHandler : BackgroundService
{
    public UserCommandHandler(
        ICommandStringFactory commandStringFactory,
        IBasicUserInputHandler basicUserInputHandler,
        IUserCommunicationHandler userCommunicationHandler,
        IBasicUserOutputHandler userOutputHandler,
        ICommandMetaData commandMetaData,
        string[] initialArguments)
    {
        _initialArguments = initialArguments;
        _commandStringFactory = commandStringFactory;
        _basicUserInputHandler = basicUserInputHandler;
        _userCommunicationHandler = userCommunicationHandler;
        _userOutputHandler = userOutputHandler;
        _commandMetaData = commandMetaData;
    }

    private void ExecuteUserCommand(string line) => ExecuteUserCommand(
        () => (_commandStringFactory.TryFromString(line, out var command), command),
        line);
    private void ExecuteUserCommand(string[] arguments) => ExecuteUserCommand(
        () => (_commandStringFactory.TryFromArguments(arguments, out var command), command),
        string.Join(' ', arguments));

    private void ExecuteUserCommand(Func<(bool, ICommand?)> tryGetCommand, string stringfiedRawObject)
    {
        _userCommunicationHandler.PromptScopeUp("CommandGenerating");

        (var commandReceived, var command) = tryGetCommand();

        // Fall back to the help command
        if (!commandReceived || command is null)
        {
            command = new HelpCommand(_userOutputHandler, _commandMetaData);
        }

        _userCommunicationHandler.PromptScopeDown();
        ExecuteUserCommand(command);
    }

    private void ExecuteUserCommand(ICommand command)
    {
        _userCommunicationHandler.PromptScopeUp(nameof(command));

        if (!command.TryExecute(out var executionException))
        {
            _userCommunicationHandler.TerminateTask($"Command execution failed: {executionException.Message}");
            return;
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
            while (true)
            {
                var userInput = _basicUserInputHandler.GetUserInput("What do you want to do? Use the \"help\" command if you're not sure!");
                ExecuteUserCommand(userInput);
            }
        }
    });

    private readonly string[] _initialArguments;
    private readonly ICommandStringFactory _commandStringFactory;
    private readonly IBasicUserInputHandler _basicUserInputHandler;
    private readonly IUserCommunicationHandler _userCommunicationHandler;
    private readonly IBasicUserOutputHandler _userOutputHandler;
    private readonly ICommandMetaData _commandMetaData;
}
