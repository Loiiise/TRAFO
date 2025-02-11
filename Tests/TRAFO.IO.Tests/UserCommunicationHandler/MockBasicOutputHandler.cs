using TRAFO.IO.Command;

namespace TRAFO.IO.Tests;
internal class MockBasicOutputHandler : IBasicUserOutputHandler
{
    public void GiveUserOutput(string output)
    {
        OutputQueue.Enqueue(output);
    }

    public Queue<string> OutputQueue { get; init; } = new();
}
