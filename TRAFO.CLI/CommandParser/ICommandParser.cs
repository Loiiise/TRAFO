namespace TRAFO.CLI;
internal interface ICommandParser
{
    ICommand Parse(string input);
    ICommand Parse(string[] arguments);

    bool TryParse(string input, out ICommand command);
    bool TryParse(string[] arguments, out ICommand command);
}
