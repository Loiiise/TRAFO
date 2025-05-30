namespace TRAFO.LocalApp.Common.Command;

public interface IBasicUserInputHandler
{
    string GetUserInput(string prompt);
    int GetNumericUserInput(string prompt);
    int GetNumericUserInput(string prompt, int lowerBound, int upperBound);
}
