namespace TRAFO.LocalApp.CLI;

internal interface IUserCommunicationHandler
{
    void TerminateTask(string message);
    void PromptScopeUp(string scopeName);
    void PromptScopeDown();
}
