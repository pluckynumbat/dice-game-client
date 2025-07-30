using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// This manager is responsible for showing the error dialogs based on different error cases
    /// </summary>
    public class ErrorManager
    {
        public ErrorStruct CurrentError { get; private set; }

        public void SetCurrentError(ErrorType errorType)
        {
            CurrentError = errors[errorType];
        }
    }

    // a struct which encapsulates an error's details
    public struct ErrorStruct
    {
        public ErrorType ErrorType;
        public ErrorSeverity Severity;
        public string ErrorTitle;
        public string ErrorMessage;
        public string ActionText;
    }
}