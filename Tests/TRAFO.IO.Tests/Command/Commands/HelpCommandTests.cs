using Shouldly;
using System.Text;
using TRAFO.IO.Command;

namespace TRAFO.IO.Tests.Command;

public class HelpCommandTests
{
    [Fact]
    public void HelpCommandHasOutputWhenExecuted()
    {
        var outputHandler = new MockBasicOutputHandler();
        var commandMetaData = new MockMetaData();
        var command = new HelpCommand(outputHandler, commandMetaData);

        outputHandler.OutputQueue.ShouldBeEmpty();

        command.Execute();
        outputHandler.OutputQueue.ShouldNotBeEmpty();
    }

    [Fact]
    public void HelpCommandHasTheSameOutputEveryTime()
    {
        var outputHandler = new MockBasicOutputHandler();
        var commandMetaData = new MockMetaData();
        var command = new HelpCommand(outputHandler, commandMetaData);

        outputHandler.OutputQueue.ShouldBeEmpty();

        command.Execute();

        var initialOutput = OutputQueueToString(outputHandler.OutputQueue);

        for (int i = 0; i < 100; ++i)
        {
            command.Execute();
            var laterResult = OutputQueueToString(outputHandler.OutputQueue);
            laterResult.ShouldBe(initialOutput);
        }

        string OutputQueueToString(Queue<string> queue)
        {
            var stringBuilder = new StringBuilder();
            while (queue.TryDequeue(out var helpOutputLine))
            {
                stringBuilder.Append(helpOutputLine);
            }
            return stringBuilder.ToString();
        }
    }
}
