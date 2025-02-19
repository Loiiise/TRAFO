using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public class CommandFlagFactory : ICommandFlagFactory
{
    public string FlagIndicator => throw new NotImplementedException();

    public ICommandFlag[] AllFromString(string value)
    {
        throw new NotImplementedException();
    }

    public ICommandFlag[] AllFromStrings(string[] values)
    {
        throw new NotImplementedException();
    }
}
