namespace TRAFO.IO.Command;

public class HelpCommand : NoArgumentAndNoFlagCommand
{
    public HelpCommand(IBasicUserOutputHandler userOutputHandler, ICommandMetaData commandMetaData)
    {
        _userOutputHandler = userOutputHandler;
        _commandMetaData = commandMetaData;
    }

    public override void Execute()
    {
        _userOutputHandler.GiveUserOutput("This is TRAFO helper. Below you'll find a list of commands along with their desciption:");

        foreach ((var commandTag, var commandDescription) in _commandMetaData.AllTagsAndDescriptions())
        {
            _userOutputHandler.GiveUserOutput($"{commandTag}: {commandDescription}");
        }
    }

    private readonly IBasicUserOutputHandler _userOutputHandler;
    private readonly ICommandMetaData _commandMetaData;
}
