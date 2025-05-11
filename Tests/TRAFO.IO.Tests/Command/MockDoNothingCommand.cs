using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Tests.Command;

internal class MockDoNothingCommand : TRAFO.IO.Command.Command
{
    public MockDoNothingCommand(ICommandFlag[] flags) : this(Array.Empty<string>(), flags) { }
    public MockDoNothingCommand(string[] arguments) : this(arguments, Array.Empty<ICommandFlag>()) { }
    public MockDoNothingCommand(string[] arguments, ICommandFlag[] flags) : base(flags) { }

    public override void Execute() { }
}
