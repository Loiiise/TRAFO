namespace TRAFO.IO.Command;

public static class CommandMetaData
{
    public static IEnumerable<(string, string, string)> AllCommandNamesTagsAndDescriptions() => _commandConfigurations.Select(c => (c.Name, c.Tag, c.Description));
    internal static IEnumerable<(string, string)> AllCommandTagsAndDescriptions() => _commandConfigurations.Select(c => (c.Tag, c.Description));

    public static string GetNameFromTag(string commandTag) => _commandConfigurations.Single(c => c.Tag == commandTag).Name;
    public static string GetTagFromName(string commandName) => _commandConfigurations.Single(c => c.Name == commandName).Tag;

    public static string GetDescriptionFromTag(string commandTag) => _commandConfigurations.Single(c => c.Tag == commandTag).Description;
    public static string GetDescriptionFromName(string commandName) => _commandConfigurations.Single(c => c.Name == commandName).Description;


    private static CommandConfiguration[] _commandConfigurations = new[]
    {
        // todo #44: this is hardcoded for now but will be extracted to a file at some point
        new CommandConfiguration(nameof(HelpCommand), "help", "This is the help command."),
        new CommandConfiguration(nameof(LoadTransactionFileCommand), "load", "Load a file from the specified path to the database."),
        new CommandConfiguration(nameof(ShowUncategorizedTransactionsCommand), "todo", "Shows all transactions that have not been placed in a category yet."),
        new CommandConfiguration(nameof(StatusCommand), "status", "Shows the current status of transaction categorization."),
    };
}

internal record CommandConfiguration(string Name, string Tag, string Description);