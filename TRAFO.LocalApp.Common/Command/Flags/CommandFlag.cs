namespace TRAFO.LocalApp.Common.Command.Flags;

public interface ICommandFlag { }
public abstract record CommandFlag<T> : ICommandFlag
{
    public required T Value { get; init; }
}
public record DateFlag : CommandFlag<DateTime> { }

public record FromFlag : DateFlag { }
public record TillFlag : DateFlag { }

public abstract record BooleanFlag : CommandFlag<bool> { }

public record SkipFirstLineFlag : BooleanFlag { }