using Model;
using Presentation.Error.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Error.Screen
{
    /// <summary>
    /// The ErrorScreen will display an error message on a popup along with a continue button
    /// </summary>
    public class ErrorScreen : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private ErrorPresenter errorPresenter;
        
        private AuthManager myAuthManager;
        private ErrorManager myErrorManager;
        private StateManager myStateManager;
        
        public void Initialize(AuthManager authManager, ErrorManager errorManager, StateManager stateManager)
        {
            myAuthManager = authManager;
            myErrorManager = errorManager;
            myStateManager = stateManager;
        }
        
        private void OnEnable()
        {
            DisplayError(myErrorManager.CurrentError);
        }

        private void OnDisable()
        {
            continueButton.onClick.RemoveAllListeners();
        }

        private void DisplayError(ErrorStruct currentError)
        {
            errorPresenter.Init(currentError.ErrorTitle, currentError.ErrorMessage, currentError.ActionText);
            Debug.Log($"Error: {currentError.ErrorTitle}: {currentError.ErrorMessage}");
        }
    }
}
