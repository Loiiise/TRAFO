namespace TRAFO.IO.Command;

public class HelpCommand : NoArgumentCommand
{
    public HelpCommand(IBasicUserOutputHandler userOutputHandler)
    {
        _userOutputHandler = userOutputHandler;
    }

    public override void Execute()
    {
        _userOutputHandler.GiveUserOutput("This is TRAFO helper. Below you'll find a list of commands along with their desciption:");

        foreach ((var commandTag, var commandDescription) in CommandMetaData.AllCommandTagsAndDescriptions())
        {
            _userOutputHandler.GiveUserOutput($"{commandTag}: {commandDescription}");
        }
    }

    private readonly IBasicUserOutputHandler _userOutputHandler;
}
