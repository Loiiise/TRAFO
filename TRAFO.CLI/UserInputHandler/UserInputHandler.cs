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