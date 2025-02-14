using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.TransactionReading;
using TRAFO.IO.TransactionWriting;
using TRAFO.Logic.Categorization;
using TRAFO.Parsing;

namespace TRAFO.IO.Command;

public class CommandFactory : ICommandFactory
{
    public CommandFactory(
        ITransactionStringReader transactionStringReader,
        ITransactionReader transactionReader,
        ITransactionWriter transactionWriter,
        ITransactionLabelUpdater transactionLabelUpdater,
        IParser parser,
        ICategorizator categorizer,
        IBasicUserInputHandler userInputHandler,
        IBasicUserOutputHandler userOutputHandler)
    {
        _transactionStringReader = transactionStringReader;
        _transactionReader = transactionReader;
        _transactionWriter = transactionWriter;
        _transactionLabelUpdater = transactionLabelUpdater;
        _parser = parser;
        _categorizer = categorizer;
        _userInputHandler = userInputHandler;
        _userOutputHandler = userOutputHandler;
    }

    public ICommand FromString(string input) => FromArguments(input.Split(' ').ToArray());

    public ICommand FromArguments(string[] arguments)
        => FromArgumentsSafe(arguments, out var command, out var exception)
        ? command
        : throw exception;

    public ICommand FromCommandNameAndArguments(string commandName, string[] arguments)
        => FromArguments(arguments.Prepend(commandName).ToArray());

    public bool TryFromString(string input, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command) => TryFromArguments(input.Split(' ').ToArray(), out command);

    public bool TryFromArguments(string[] arguments, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command)
        => FromArgumentsSafe(arguments, out command, out var _);

    public bool TryFromCommandNameAndArguments(string commandName, string[] arguments, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command)
        => TryFromArguments(arguments.Prepend(commandName).ToArray(), out command);

    protected bool FromArgumentsSafe(string[] arguments, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        if (!arguments.Any())
        {
            command = null;
            exception = new ArgumentException("No command was provided");
            return false;
        }

        var commandName = CommandMetaData.GetNameFromTag(arguments[0]);

        command = GetCommand(commandName, arguments.Skip(1).ToArray());
        exception = null;
        return true;
    }

    private ICommand GetCommand(string commandName, string[] args) => commandName switch
    {
        nameof(HelpCommand) => new HelpCommand(_userOutputHandler),
        nameof(LoadTransactionFileCommand) => new LoadTransactionFileCommand(_transactionStringReader, _parser, new[] { _categorizer }, _transactionWriter, args),
        nameof(ProcessUncategorizedTransactionsCommand) => new ProcessUncategorizedTransactionsCommand(_userInputHandler,_userOutputHandler, _transactionReader, _transactionLabelUpdater),
        nameof (ShowUncategorizedTransactionsCommand) => new ShowUncategorizedTransactionsCommand(_transactionReader, _userOutputHandler),
        nameof(StatusCommand) => new StatusCommand(_transactionReader, _userOutputHandler),
        _ => throw new NotImplementedException(),
    };

    private readonly ITransactionStringReader _transactionStringReader;
    private readonly ITransactionReader _transactionReader;
    private readonly ITransactionWriter _transactionWriter;
    private readonly ITransactionLabelUpdater _transactionLabelUpdater;
    private readonly IParser _parser;
    private readonly ICategorizator _categorizer;
    private readonly IBasicUserInputHandler _userInputHandler;
    private readonly IBasicUserOutputHandler _userOutputHandler;
}
