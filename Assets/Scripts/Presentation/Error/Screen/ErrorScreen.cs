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
        
        public void Initialize(AuthManager authManager, ErrorManager errorManager)
        {
            myAuthManager = authManager;
            myErrorManager = errorManager;
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

            switch (currentError.Severity)
            {
                case ErrorSeverity.Basic:
                    continueButton.onClick.AddListener(DismissErrorScreen);
                    break;
                
                case ErrorSeverity.Reload:
                    continueButton.onClick.AddListener(ReloadTheGame);
                    break;
                
                case ErrorSeverity.Quit:
                    continueButton.onClick.AddListener(QuitTheApp);
                    break;
            }
        }

        private void DismissErrorScreen()
        {
            myErrorManager.ExitErrorState();
        }

        private async void ReloadTheGame()
        {
            await myAuthManager.RequestLogout();
            GameRoot.Instance.ReloadTheGame();
        }

        private void QuitTheApp()
        {
            Debug.LogError("Quitting the app"); // added this since we cannot quit the app in the editor
            Application.Quit();
        }
    }
}
