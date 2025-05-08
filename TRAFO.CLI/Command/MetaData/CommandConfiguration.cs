namespace TRAFO.CLI.Command.MetaData;

internal abstract record CommandOrFlagConfiguration(string Name, string Tag, string Description);

internal record CommandConfiguration(string Name, string Tag, string Description) : CommandOrFlagConfiguration(Name, Tag, Description) { }
internal record FlagConfiguration(string Name, string Tag, string Description) : CommandOrFlagConfiguration(Name, Tag, Description) { }

internal interface IMetaData
{
    IEnumerable<(string, string, string)> AllNamesTagsAndDescriptions();
    IEnumerable<(string, string)> AllTagsAndDescriptions();

    string GetNameFromTag(string commandTag);
    string GetTagFromName(string commandName);

    string GetDescriptionFromTag(string commandTag);
    string GetDescriptionFromName(string commandName);
}

internal interface IMetaData<T> : IMetaData
    where T : CommandOrFlagConfiguration
{ }

internal interface ICommandMetaData : IMetaData<CommandConfiguration> { }
internal interface IFlagMetaData : IMetaData<FlagConfiguration> { }

internal abstract class MetaData<T> : IMetaData<T>
    where T : CommandOrFlagConfiguration
{
    public IEnumerable<(string, string, string)> AllNamesTagsAndDescriptions() => _commandConfigurations.Select(c => (c.Name, c.Tag, c.Description));
    public IEnumerable<(string, string)> AllTagsAndDescriptions() => _commandConfigurations.Select(c => (c.Tag, c.Description));

    public string GetNameFromTag(string commandTag) => _commandConfigurations.Single(c => c.Tag == commandTag).Name;
    public string GetTagFromName(string commandName) => _commandConfigurations.Single(c => c.Name == commandName).Tag;

    public string GetDescriptionFromTag(string commandTag) => _commandConfigurations.Single(c => c.Tag == commandTag).Description;
    public string GetDescriptionFromName(string commandName) => _commandConfigurations.Single(c => c.Name == commandName).Description;

    protected abstract T[] _commandConfigurations { get; }
}
