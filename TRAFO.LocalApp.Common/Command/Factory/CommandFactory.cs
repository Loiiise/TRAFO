using System.Diagnostics.CodeAnalysis;
using TRAFO.LocalApp.Common.Command.Arguments;
using TRAFO.LocalApp.Common.Command.Flags;
using TRAFO.LocalApp.Common.FileReading;
using TRAFO.Logic.Categorization;
using TRAFO.Logic.Extensions;
using TRAFO.Repositories.Interfaces;
using TRAFO.Services.Parser;

namespace TRAFO.LocalApp.Common.Command;

public class CommandFactory : ICommandFactory
{
    public CommandFactory(
        ITransactionFileReader transactionFileReader,
        ITransactionReader transactionReader,
        IBalanceReader balanceReader,
        IBalanceWriter balanceWriter,
        ILabelReader labelReader,
        ITransactionWriter transactionWriter,
        ITransactionLabelUpdater transactionLabelUpdater,
        IParser parser,
        ILabelApplier categorizer,
        IBasicUserInputHandler userInputHandler,
        IBasicUserOutputHandler userOutputHandler)
    {
        _transactionFileReader = transactionFileReader;
        _transactionReader = transactionReader;
        _balanceReader = balanceReader;
        _balanceWriter = balanceWriter;
        _labelReader = labelReader;
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
        command = commandName switch
        {
            nameof(GetBalanceCommand) =>
                args.TryGetFirstOrDefault<IdentifierArgument>(out var identifierArgument) ?
                    new GetBalanceCommand(_balanceReader, flags)
                    {
                        IdentifierArgument = identifierArgument,
                    } : new ArgumentNotFoundCommand(),
            nameof(SetBalanceCommand) =>
                args.TryGetFirstOrDefault<AmountArgument>(out var amountArgument) &&
                args.TryGetFirstOrDefault<CurrencyArgument>(out var currencyArgument) &&
                args.TryGetFirstOrDefault<IdentifierArgument>(out var identifierArgument) ?
                    new SetBalanceCommand(_balanceWriter, flags)
                    {
                        AmountArgument = amountArgument,
                        CurrencyArgument = currencyArgument,
                        IdentifierArgument = identifierArgument,
                    } : new ArgumentNotFoundCommand(),
            nameof(LoadTransactionFileCommand) =>
                args.TryGetFirstOrDefault<FilePathArgument>(out var filePathArgument) ?
                    new LoadTransactionFileCommand(_transactionFileReader, new[] { _categorizer }, _transactionWriter, flags)
                    {
                        FilePathArgument = filePathArgument
                    } : new ArgumentNotFoundCommand(),
            nameof(ProcessUncategorizedTransactionsCommand) => new ProcessUncategorizedTransactionsCommand(_userInputHandler, _labelReader, _transactionReader, _transactionLabelUpdater, flags),
            nameof(ReportCommand) => new ReportCommand(_transactionReader, _userOutputHandler, flags),
            nameof(ShowUncategorizedTransactionsCommand) => new ShowUncategorizedTransactionsCommand(_transactionReader, _userOutputHandler, flags),
            nameof(StatusCommand) => new StatusCommand(_transactionReader, _userOutputHandler, flags),
            _ => null,
        };

        exception = command switch
        {
            ArgumentNotFoundCommand => new ArgumentException($"Missing arguments for {commandName}"),
            null => new ArgumentException($"Command {commandName} is a valid argument name"),
            _ => null,
        };

        return exception == null;
    }

    private class ArgumentNotFoundCommand : ICommand
    {
        public ICommandFlag[] Flags { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

        public void Execute() => throw new NotSupportedException();
        public bool TryExecute([MaybeNullWhen(true), NotNullWhen(false)] out Exception exception) => throw new NotSupportedException();
    }

    private readonly ITransactionFileReader _transactionFileReader;
    private readonly ITransactionReader _transactionReader;
    private readonly IBalanceReader _balanceReader;
    private readonly IBalanceWriter _balanceWriter;
    private readonly ILabelReader _labelReader;
    private readonly ITransactionWriter _transactionWriter;
    private readonly ITransactionLabelUpdater _transactionLabelUpdater;
    private readonly IParser _parser;
    private readonly ILabelApplier _categorizer;
    private readonly IBasicUserInputHandler _userInputHandler;
    private readonly IBasicUserOutputHandler _userOutputHandler;
}
