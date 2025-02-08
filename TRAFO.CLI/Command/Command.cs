namespace TRAFO.CLI;

internal abstract class Command : ICommand
{
    internal Command(IUserCommunicationHandler userInputHandler)
    {
        _userInputHandler = userInputHandler;
    }

    public abstract string Name { get; }
    public abstract string[] Arguments { get; init; }
    internal abstract int ExpectedAmountOfArguments { get; }

    public bool Validate()
    {
        if (Arguments.Length != ExpectedAmountOfArguments)
        {
            _userInputHandler.TerminateTask($"Expected {ExpectedAmountOfArguments} arguments, received {Arguments.Length}");
            return false;
        }

        try
        {
            ValidateInternally(_userInputHandler);
        }
        catch (Exception ex)
        {
            _userInputHandler.TerminateTask($"Command validation failed: {ex.Message}");
            return false;
        }

        return true;
    }
    internal abstract void ValidateInternally(IBasicUserInputHandler userInputHandler);

    public bool Execute()
    {        
        try
        {
            ExecuteInternally(_userInputHandler);
        }
        catch (Exception ex)
        {
            _userInputHandler.TerminateTask($"Command execution failed: {ex.Message}");
            return false;
        }

        return true;
    }
    internal abstract bool ExecuteInternally(IBasicUserInputHandler userInputHandler);

    private readonly IUserCommunicationHandler _userInputHandler;
}
