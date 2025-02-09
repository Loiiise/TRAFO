namespace TRAFO.IO.Command;

internal interface ICommandFactory
{
    ICommand FromString(string input);
    ICommand FromArguments(string[] arguments);

    bool TryFromsString(string input, out ICommand command);
    bool TryFromArguments(string[] arguments, out ICommand command);
}
