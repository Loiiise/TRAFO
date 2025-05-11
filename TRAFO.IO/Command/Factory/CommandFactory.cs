using System.Diagnostics.CodeAnalysis;
using TRAFO.IO.Command.Arguments;
using TRAFO.IO.Command.Flags;
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
        ICategoryReader categoryReader,
        ITransactionWriter transactionWriter,
        ITransactionLabelUpdater transactionLabelUpdater,
        IParser parser,
        ICategorizator categorizer,
        IBasicUserInputHandler userInputHandler,
        IBasicUserOutputHandler userOutputHandler)
    {
        _transactionStringReader = transactionStringReader;
        _transactionReader = transactionReader;
        _categoryReader = categoryReader;
        _transactionWriter = transactionWriter;
        _transactionLabelUpdater = transactionLabelUpdater;
        _parser = parser;
        _categorizer = categorizer;
        _userInputHandler = userInputHandler;
        _userOutputHandler = userOutputHandler;
    }

    public ICommand GetCommand(string commandName, ICommandArgument[] args, ICommandFlag[] flags)
        => TryGetCommandSafe(commandName, args, flags, out var command, out var exception) ?
            command :
            throw exception;

    public bool TryGetCommand(string commandName, ICommandArgument[] args, ICommandFlag[] flags, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command)
        => TryGetCommandSafe(commandName, args, flags, out command, out var _);

    private bool TryGetCommandSafe(string commandName, ICommandArgument[] args, ICommandFlag[] flags, [MaybeNullWhen(false), NotNullWhen(true)] out ICommand command, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        exception = null;

        // Commands without arguments
        command = commandName switch
        {
            nameof(ProcessUncategorizedTransactionsCommand) => new ProcessUncategorizedTransactionsCommand(_userInputHandler, _categoryReader, _transactionReader, _transactionLabelUpdater, flags),
            nameof(ReportCommand) => new ReportCommand(_transactionReader, _userOutputHandler, flags),
            nameof(ShowUncategorizedTransactionsCommand) => new ShowUncategorizedTransactionsCommand(_transactionReader, _userOutputHandler, flags),
            nameof(StatusCommand) => new StatusCommand(_transactionReader, _userOutputHandler, flags),
            _ => null,
        };

        // Commands with arguments
        if (commandName == nameof(LoadTransactionFileCommand))
        {
            if (args.Length == 1 && args[0] is FilePathArgument filePathArgument)
            {
                command = new LoadTransactionFileCommand(_transactionStringReader, _parser, new[] { _categorizer }, _transactionWriter, flags)
                {
                    FilePathArgument = filePathArgument
                };
            }
            else
            {
                exception = new ArgumentException("No file path was provided");
            }
        }

        if (command == null)
        {
            exception = new ArgumentException($"Command {commandName} is a valid argument name");
        }

        return exception == null;
    }

    private readonly ITransactionStringReader _transactionStringReader;
    private readonly ITransactionReader _transactionReader;
    private readonly ICategoryReader _categoryReader;
    private readonly ITransactionWriter _transactionWriter;
    private readonly ITransactionLabelUpdater _transactionLabelUpdater;
    private readonly IParser _parser;
    private readonly ICategorizator _categorizer;
    private readonly IBasicUserInputHandler _userInputHandler;
    private readonly IBasicUserOutputHandler _userOutputHandler;
}
