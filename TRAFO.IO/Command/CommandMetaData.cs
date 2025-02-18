namespace TRAFO.IO.Command;

internal class CommandMetaData : MetaData<CommandConfiguration>, ICommandMetaData
{
    protected override CommandConfiguration[] _commandConfigurations => new[]
    {
        // todo #44: this is hardcoded for now but will be extracted to a file at some point
        new CommandConfiguration(nameof(HelpCommand), "help", "This is the help command."),
        new CommandConfiguration(nameof(LoadTransactionFileCommand), "load", "Load a file from the specified path to the database."),
        new CommandConfiguration(nameof(ProcessUncategorizedTransactionsCommand), "process", "Manually set the category of transactions that have not been placed in a category yet."),
        new CommandConfiguration(nameof(ShowUncategorizedTransactionsCommand), "todo", "Shows all transactions that have not been placed in a category yet."),
        new CommandConfiguration(nameof(StatusCommand), "status", "Shows the current status of transaction categorization."),
    };
}
