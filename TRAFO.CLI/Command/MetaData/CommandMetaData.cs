using TRAFO.IO.Command;
using TRAFO.IO.Command.Commands;

namespace TRAFO.CLI.Command.MetaData;

internal class CommandMetaData : MetaData<CommandConfiguration>, ICommandMetaData
{
    protected override CommandConfiguration[] _commandConfigurations => new[]
    {
        // todo #44: this is hardcoded for now but will be extracted to a file at some point
        new CommandConfiguration(nameof(HelpCommand), "help", "This is the help command."),
        new CommandConfiguration(nameof(SetBalanceCommand), "balance", "Sets the balance at a given point in time."),
        new CommandConfiguration(nameof(LoadTransactionFileCommand), "load", "Load a file from the specified path to the database."),
        new CommandConfiguration(nameof(ProcessUncategorizedTransactionsCommand), "process", "Manually set the category of transactions that have not been placed in a category yet."),
        new CommandConfiguration(nameof(ReportCommand), "report", "Get a report of your spendings."),
        new CommandConfiguration(nameof(ShowUncategorizedTransactionsCommand), "todo", "Shows all transactions that have not been placed in a category yet."),
        new CommandConfiguration(nameof(StatusCommand), "status", "Shows the current status of transaction categorization."),
    };
}
