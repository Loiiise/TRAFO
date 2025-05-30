using TRAFO.LocalApp.Common.Command.Flags;

namespace TRAFO.LocalApp.Common.Tests.Command;

internal class MockDoNothingCommand : Common.Command.Command
{
    public MockDoNothingCommand(ICommandFlag[] flags) : this(Array.Empty<string>(), flags) { }
    public MockDoNothingCommand(string[] arguments) : this(arguments, Array.Empty<ICommandFlag>()) { }
    public MockDoNothingCommand(string[] arguments, ICommandFlag[] flags) : base(flags) { }

    public override void Execute() { }
}
