using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TRAFO.IO.Command;

namespace TRAFO.CLI;
internal class UserCommandHandler : BackgroundService
{
    public UserCommandHandler(
        ILogger<UserCommandHandler> logger, 
        ICommandFactory commandFactory, 
        IUserCommunicationHandler userCommunicationHandler,
        string[] initialArguments)
    {
        _initialArguments = initialArguments;
        _logger = logger;
        _commandFactory = commandFactory;
        _userCommunicationHandler = userCommunicationHandler;
    }

    public void ExecuteUserCommand(string[] arguments)
    {
        _userCommunicationHandler.PromptScopeUp("CommandGenerating");

        if (_commandFactory.TryFromArguments(arguments, out var command))
        {
            _userCommunicationHandler.PromptScopeDown();
            ExecuteUserCommand(command);
            return;
        }

        _userCommunicationHandler.TerminateTask($"Failed to parse \"{string.Join(' ', arguments)}\"");
    }

    public void ExecuteUserCommand(ICommand command)
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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Handler started with initial arguments: {string.Join(' ', _initialArguments)}");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker command handler running at: {time}", DateTimeOffset.Now);

            await Task.Delay(1_000, stoppingToken);

        }
    }

    private readonly string[] _initialArguments;
    private readonly ILogger<UserCommandHandler> _logger;
    private readonly ICommandFactory _commandFactory;
    private readonly IUserCommunicationHandler _userCommunicationHandler;
}
