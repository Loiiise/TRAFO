namespace TRAFO.CLI;

internal interface IBasicUserInputHandler
{
    string GetUserInput(string prompt);
    int GetNumericUserInput(string prompt);
}

internal interface IUserCommunicationHandler : IBasicUserInputHandler
{
    void TerminateTask(string message);
}

internal interface ICLIUserInputHandler : IUserCommunicationHandler
{
    void PromptScopeUp(string subScopeName);
    void PromptScopeDown();
}

internal class NotImplementedCLIUserInputHandler : ICLIUserInputHandler
{
    public int GetNumericUserInput(string prompt)
    {
        throw new NotImplementedException();
    }

    public string GetUserInput(string prompt)
    {
        throw new NotImplementedException();
    }

    public void PromptScopeDown()
    {
        throw new NotImplementedException();
    }

    public void PromptScopeUp(string subScopeName)
    {
        throw new NotImplementedException();
    }

    public void TerminateTask(string message)
    {
        throw new NotImplementedException();
    }
}