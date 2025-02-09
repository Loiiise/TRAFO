using TRAFO.IO.Command.Commands;

namespace TRAFO.IO.Command;

public class HelpCommand : NoArgumentCommand
{
    public HelpCommand(IBasicUserOutputHandler userOutputHandler, ICommandFactory commandFactory)
    {
        _userOutputHandler = userOutputHandler;
        _commandFactory = commandFactory;
    }

    public override void Execute()
    {
        _userOutputHandler.GiveUserOutput("This is TRAFO helper. Below you'll find a list of commands along with their desciption:");
        foreach (var commandName in _commandFactory.AllCommandNames())
        {
            var tag = _commandFactory.GetTag(commandName);
            var description = _commandFactory.GetDescription(commandName);
            _userOutputHandler.GiveUserOutput($"{tag}: {description}");
        }
    }

    private readonly IBasicUserOutputHandler _userOutputHandler;
    private readonly ICommandFactory _commandFactory;
}
