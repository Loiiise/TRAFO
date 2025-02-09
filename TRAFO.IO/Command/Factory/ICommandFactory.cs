namespace TRAFO.IO.Command;

public interface ICommandFactory
{
    ICommand FromString(string input);
    ICommand FromArguments(string[] arguments);
    ICommand FromCommandNameAndArguments(string commandName, string[] arguments);

    bool TryFromString(string input, out ICommand command);
    bool TryFromArguments(string[] arguments, out ICommand command);
    bool TryFromCommandNameAndArguments(string commandName, string[] arguments, out ICommand command);

    IEnumerable<string> AllCommandNames();
    
    string GetTag(string commandName);
    string GetTag(ICommand command);
    
    string GetDescription(string commandName);
    string GetDescription(ICommand command);
}
