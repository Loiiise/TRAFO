namespace TRAFO.IO.Command.Flags;

public abstract record CommandFlag<T> { }
public abstract record DateFlag : CommandFlag<DateTime> { }

public record FromFlag : DateFlag { }
public record TillFlag : DateFlag { }
