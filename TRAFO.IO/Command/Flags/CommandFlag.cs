namespace TRAFO.IO.Command.Flags;

public interface ICommandFlag { }
public abstract record CommandFlag<T> : ICommandFlag { }
public abstract record DateFlag : CommandFlag<DateTime> { }

public record FromFlag : DateFlag { }
public record TillFlag : DateFlag { }
