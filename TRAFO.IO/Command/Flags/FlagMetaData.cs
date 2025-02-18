namespace TRAFO.IO.Command.Flags;

internal class FlagMetaData : MetaData<FlagConfiguration>, IMetaDataFlagConfiguration
{
    protected override FlagConfiguration[] _commandConfigurations => new[]
    {
        // todo #44: this is hardcoded for now but will be extracted to a file at some point
        new FlagConfiguration(string.Empty, "from", "start date of the range in a YYYY-MM-DD format."),
        new FlagConfiguration(string.Empty, "till", "end date of the range in a YYYY-MM-DD format."),
    };
}
