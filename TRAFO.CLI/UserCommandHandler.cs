using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TRAFO.IO.Command;

namespace TRAFO.CLI;
internal class UserCommandHandler : BackgroundService
{
    public UserCommandHandler(ILogger<UserCommandHandler> logger, ICommandFactory commandFactory, ICLIUserInputHandler userInputHandler)
    {
        _logger = logger;
        _commandFactory = commandFactory;
        _userInputHandler = userInputHandler;
    }

    public void ExecuteUserCommand(string[] arguments)
    {
        if (_commandFactory.TryFromArguments(arguments, out var command))
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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker command handler running at: {time}", DateTimeOffset.Now);

            await Task.Delay(1_000, stoppingToken);

        }
    }

    private readonly ILogger<UserCommandHandler> _logger;
    private readonly ICommandFactory _commandFactory;
    private readonly ICLIUserInputHandler _userInputHandler;
}
