namespace TRAFO.IO.Command.Commands;

public abstract class NoArgumentCommand : Command
{
    protected NoArgumentCommand() : base(Array.Empty<string>()) { }

    internal override sealed int _expectedAmountOfArguments => 0;
    internal override sealed void ValidateInternally() { }
}
