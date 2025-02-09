namespace TRAFO.CLI;

internal class ConsoleUserInputHandler : IBasicUserInputHandler, IUserCommunicationHandler
{
    public int GetNumericUserInput(string prompt)
    {
        int result;

        while (!int.TryParse(GetUserInput(prompt), out result))
        {
            ShowLine("Please enter a number.");
        }

        return result;
    }

    public int GetNumericUserInput(string prompt, int lowerBound, int upperBound)
    {
        int result = GetNumericUserInput(prompt);

        while (result < lowerBound || result > upperBound)
        {
            ShowLine($"Please enter a number between {lowerBound} and {upperBound} (inclusive bounds).");
            result = GetNumericUserInput(prompt);
        }

        return result;
    }

    public string GetUserInput(string prompt)
    {
        ShowLine(prompt);
        return GetLine();
    }

    public void TerminateTask(string message)
    {
        ShowLine("TASK FAILED");
        ShowLine(message);
        PromptScopeDown();
    }

    public void PromptScopeUp(string scopeName) => _promptStack.Push(scopeName);
    public void PromptScopeDown() => _promptStack.TryPop(out var _);

    private void ShowLine(string message) => Console.WriteLine(message);
    private string GetLine()
    {
        Console.Write($"{(_promptStack.Any() ? _promptStack.Peek() : string.Empty)}>");
        return Console.ReadLine() ?? string.Empty;
    }

    private Stack<string> _promptStack = new();
}
