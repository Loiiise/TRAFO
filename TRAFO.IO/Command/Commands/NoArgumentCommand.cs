namespace TRAFO.IO.Command;

public abstract class NoArgumentCommand : Command
{
    protected NoArgumentCommand() : base(Array.Empty<string>()) { }

    protected override sealed int _expectedAmountOfArguments => 0;
    protected override sealed void ValidateInternally() { }
}
