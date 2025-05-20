using TRAFO.Logic;

namespace TRAFO.IO.Command.Arguments;

public interface ICommandArgument { }

public abstract record CommandArgument<T> : ICommandArgument
{
    public required T Value { get; set; }
}

public abstract record StringArgument : CommandArgument<string> { }
public abstract record LongArgument : CommandArgument<long> { }


public record FilePathArgument : StringArgument { }
public record AmountArgument : LongArgument { }
public record CurrencyArgument : CommandArgument<Currency> { }
public record IdentifierArgument : StringArgument { }