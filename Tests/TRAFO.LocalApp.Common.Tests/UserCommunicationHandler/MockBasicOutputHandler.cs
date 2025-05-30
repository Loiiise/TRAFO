using TRAFO.LocalApp.Common.Command;

namespace TRAFO.LocalApp.Common.Tests;
public class MockBasicOutputHandler : IBasicUserOutputHandler
{
    public void GiveUserOutput(string output)
    {
        OutputQueue.Enqueue(output);
    }

    public Queue<string> OutputQueue { get; init; } = new();
}
