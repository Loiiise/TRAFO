namespace TRAFO.CLI;
internal interface ICommandFactory
{
    ICommand Parse(string input);
    ICommand Parse(string[] arguments);

    bool TryParse(string input, out ICommand command);
    bool TryParse(string[] arguments, out ICommand command);
}

internal class NotImplementedCommandFactory() : ICommandFactory
{
    public ICommand Parse(string input)
    {
        throw new NotImplementedException();
    }

    public ICommand Parse(string[] arguments)
    {
        throw new NotImplementedException();
    }

    public bool TryParse(string input, out ICommand command)
    {
        throw new NotImplementedException();
    }

    public bool TryParse(string[] arguments, out ICommand command)
    {
        throw new NotImplementedException();
    }
}