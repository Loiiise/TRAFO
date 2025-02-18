using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public class HelpCommand : Command
{
    public HelpCommand(IBasicUserOutputHandler userOutputHandler, ICommandMetaData commandMetaData) : base(Array.Empty<string>(), Array.Empty<ICommandFlag>())
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

    protected override void ValidateInternally() { }
    protected override bool IsSupported(ICommandFlag flag) => false;
    protected override int _expectedAmountOfArguments =>0;

    private readonly IBasicUserOutputHandler _userOutputHandler;
    private readonly ICommandMetaData _commandMetaData;

}
