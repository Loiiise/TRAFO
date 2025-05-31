using System.Diagnostics.CodeAnalysis;
using TRAFO.LocalApp.Common.Command.Arguments;
using TRAFO.Logic.Dto;

namespace TRAFO.LocalApp.Common.Command.Factory;
public class CommandArgumentFactory : ICommandArgumentFactory
{
    public ICommandArgument GetArgument(string commandName, string argumentValue)
        => GetArgumentSafe(commandName, argumentValue, out var argument, out var exception) ?
            argument :
            throw exception;

    public bool TryGetArgument(string commandName, string argumentValue, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandArgument argument)
        => GetArgumentSafe(commandName, argumentValue, out argument, out _);

    public bool GetArgumentSafe(string commandName, string argumentValue, [MaybeNullWhen(false), NotNullWhen(true)] out ICommandArgument argument, [MaybeNullWhen(true), NotNullWhen(false)] out Exception exception)
    {
        argument = null;
        exception = null;

        if (commandName == nameof(LoadTransactionFileCommand))
        {
            argument = new FilePathArgument
            {
                Value = argumentValue,
            };
        }
        if (commandName == nameof(GetBalanceCommand))
        {
            argument = new IdentifierArgument { Value = argumentValue };
        }
        if (commandName == nameof(SetBalanceCommand))
        {
            argument =
                long.TryParse(argumentValue, out var balanceValue) ? new AmountArgument { Value = balanceValue } :
                Enum.TryParse<Currency>(argumentValue, out var currencyValue) ? new CurrencyArgument { Value = currencyValue } :
                new IdentifierArgument { Value = argumentValue };
        }

        if (argument == null)
        {
            exception = new NotSupportedException($"No valid argument could be parsed for {commandName} with value {argumentValue}");
        }

        return exception == null;
    }
}
