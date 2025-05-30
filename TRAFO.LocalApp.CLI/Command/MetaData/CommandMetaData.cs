using TRAFO.LocalApp.Common.Command;

namespace TRAFO.LocalApp.CLI.Command.MetaData;

internal class CommandMetaData : MetaData<CommandConfiguration>, ICommandMetaData
{
    protected override CommandConfiguration[] _commandConfigurations => new[]
    {
        // todo #44: this is hardcoded for now but will be extracted to a file at some point
        new CommandConfiguration(nameof(HelpCommand), "help", "This is the help command."),
        new CommandConfiguration(nameof(GetBalanceCommand), "get-balance", "Gets the balance at a given point in time."),
        new CommandConfiguration(nameof(SetBalanceCommand), "set-balance", "Sets the balance at a given point in time."),
        new CommandConfiguration(nameof(LoadTransactionFileCommand), "load", "Load a file from the specified path to the database."),
        new CommandConfiguration(nameof(ProcessUncategorizedTransactionsCommand), "process", "Manually set a label for transactions that have not been labeled yet."),
        new CommandConfiguration(nameof(ReportCommand), "report", "Get a report of your spendings."),
        new CommandConfiguration(nameof(ShowUncategorizedTransactionsCommand), "todo", "Shows all transactions that have not been labeled yet."),
        new CommandConfiguration(nameof(StatusCommand), "status", "Shows the current status of transaction categorization."),
    };
}
