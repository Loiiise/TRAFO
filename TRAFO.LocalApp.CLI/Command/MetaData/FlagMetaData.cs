using TRAFO.LocalApp.Common.Command.Flags;

namespace TRAFO.LocalApp.CLI.Command.MetaData;

public class FlagMetaData : MetaData<FlagConfiguration>, IFlagMetaData
{
    protected override FlagConfiguration[] _commandConfigurations => new[]
    {
        // todo #44: this is hardcoded for now but will be extracted to a file at some point
        new FlagConfiguration(nameof(FromFlag), "from", "start date of the range in a YYYY-MM-DD format."),
        new FlagConfiguration(nameof(TillFlag), "till", "end date of the range in a YYYY-MM-DD format."),
        new FlagConfiguration(nameof(DateFlag), "at", "timestamp in a YYYY-MM-DD format."),
    };
}
