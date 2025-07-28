using Model;
using Presentation.Results.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Results.Screen
{
    public class ResultsScreen : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private ResultsPresenter resultsPresenter;
        
        private GameplayManager myGameplayManager;
        private StateManager myStateManager;
        
        public void Initialize(GameplayManager gameplayManager, StateManager  stateManager)
        {
            myGameplayManager = gameplayManager;
            myStateManager = stateManager;
        }
        
        private void OnEnable()
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked);
            string resultsText = myGameplayManager.WonLastPlayedLevel ? "Level Won!" : "Level Lost";
            resultsPresenter.Init(resultsText);
        }

        private void OnDisable()
        {
            continueButton.onClick.RemoveAllListeners();
        }

        private void OnContinueButtonClicked()
        {
            myStateManager.ChangeGameState(StateManager.GameState.MainMenu);
        }
    }
}
