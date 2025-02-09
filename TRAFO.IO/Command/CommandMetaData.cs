namespace TRAFO.IO.Command;

internal static class CommandMetaData
{
    internal static IEnumerable<(string, string)> AllCommandTagsAndDescriptions() => _commandConfigurations.Select(c => (c.Tag, c.Description));

    internal static string GetNameFromTag(string commandTag) => _commandConfigurations.Single(c => c.Tag == commandTag).Name;
    internal static string GetTagFromName(string commandName) => _commandConfigurations.Single(c => c.Name == commandName).Tag;

    internal static string GetDescriptionFromTag(string commandTag) => _commandConfigurations.Single(c => c.Tag == commandTag).Description;
    internal static string GetDescriptionFromName(string commandName) => _commandConfigurations.Single(c => c.Name == commandName).Description;


    private static CommandConfiguration[] _commandConfigurations = new[]
    {
        // todo #44: this is hardcoded for now but will be extracted to a file at some point
        new CommandConfiguration(nameof(HelpCommand), "help", "This is the help command"),
    };
}

internal record CommandConfiguration(string Name, string Tag, string Description);