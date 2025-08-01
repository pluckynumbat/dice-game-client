using System.Collections.Generic;

namespace Model
{
    public enum ErrorType
    {
        None = 0,
        NotEnoughEnergy,
        CouldNotConnect,
        ServerTimeout,
        Unauthorized,
        CriticalError,
    }

    public enum ErrorSeverity
    {
        None = 0,
        Basic,
        Reload,
        Quit,
    }
    
    /// <summary>
    /// This manager is responsible for showing the error dialogs based on different error cases
    /// </summary>
    public class ErrorManager
    {
        public ErrorStruct CurrentError { get; private set; }
        
        private readonly Dictionary<ErrorType, ErrorStruct> errors;
        private StateManager.GameState previousGameState;

        public ErrorManager()
        {
            errors = new Dictionary<ErrorType, ErrorStruct>
            {
                [ErrorType.None] = new () { ErrorType = ErrorType.None, Severity = ErrorSeverity.Basic, ErrorTitle = "Test Title", ErrorMessage = "This is a test description of an error", ActionText = "OK"},
                [ErrorType.NotEnoughEnergy] = new () { ErrorType = ErrorType.NotEnoughEnergy, Severity = ErrorSeverity.Basic, ErrorTitle = "Low Energy", ErrorMessage = "please try again later",  ActionText = "OK"},
                [ErrorType.Unauthorized] = new () { ErrorType = ErrorType.Unauthorized, Severity = ErrorSeverity.Reload, ErrorTitle = "Unauthorized", ErrorMessage = "the session has expired, please reload", ActionText = "Reload"},
                [ErrorType.CouldNotConnect] = new () { ErrorType = ErrorType.CouldNotConnect, Severity = ErrorSeverity.Basic, ErrorTitle = "Cannot Connect", ErrorMessage = "please check your connection, and try again later", ActionText = "Quit"},
                [ErrorType.ServerTimeout] = new () { ErrorType = ErrorType.ServerTimeout, Severity = ErrorSeverity.Quit, ErrorTitle = "Time Out", ErrorMessage = "the server did not respond,  please try again later", ActionText = "Quit"},
                [ErrorType.CriticalError] = new () { ErrorType = ErrorType.CriticalError, Severity = ErrorSeverity.Quit, ErrorTitle = "Critical Error", ErrorMessage = "please quit the app, and try again later", ActionText = "Quit"},
            };
        }
        
        public void EnterErrorState(ErrorType errorType)
        {
            previousGameState = GameRoot.Instance.StateManager.CurrentState;
            CurrentError = errors[errorType];
            GameRoot.Instance.StateManager.ChangeGameState(StateManager.GameState.Error);
        }
        
        public void ExitErrorState()
        {
            CurrentError = errors[ErrorType.None];
            GameRoot.Instance.StateManager.ChangeGameState(previousGameState);
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