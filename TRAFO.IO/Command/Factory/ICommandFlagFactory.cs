using TRAFO.IO.Command.Flags;

namespace TRAFO.IO.Command;

public interface ICommandFlagFactory
{
    ICommandFlag[] AllFromString(string value);
    ICommandFlag[] AllFromStrings(string[] values);

    string FlagIndicator { get; }
}
