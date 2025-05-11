namespace TRAFO.IO.Command.Arguments;

public interface ICommandArgument { }

public abstract class CommandArgument<T> : ICommandArgument
{
    public required T Value { get; set; }
}

public class FilePathArgument : CommandArgument<string> { }
