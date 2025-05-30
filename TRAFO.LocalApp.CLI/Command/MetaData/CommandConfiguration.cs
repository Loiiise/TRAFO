namespace TRAFO.LocalApp.CLI.Command.MetaData;

public abstract record CommandOrFlagConfiguration(string Name, string Tag, string Description);

public record CommandConfiguration(string Name, string Tag, string Description) : CommandOrFlagConfiguration(Name, Tag, Description) { }
public record FlagConfiguration(string Name, string Tag, string Description) : CommandOrFlagConfiguration(Name, Tag, Description) { }

public interface IMetaData
{
    IEnumerable<(string, string, string)> AllNamesTagsAndDescriptions();
    IEnumerable<(string, string)> AllTagsAndDescriptions();

    string GetNameFromTag(string commandTag);
    string GetTagFromName(string commandName);

    string GetDescriptionFromTag(string commandTag);
    string GetDescriptionFromName(string commandName);
}

public interface IMetaData<T> : IMetaData
    where T : CommandOrFlagConfiguration
{ }

public interface ICommandMetaData : IMetaData<CommandConfiguration> { }
public interface IFlagMetaData : IMetaData<FlagConfiguration> { }

public abstract class MetaData<T> : IMetaData<T>
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
