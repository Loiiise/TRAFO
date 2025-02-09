namespace TRAFO.CLI;

internal interface ICommand
{
    public string Name { get; }
    public string[] Arguments { get; init; }

    bool Validate();
    bool Execute();
}
