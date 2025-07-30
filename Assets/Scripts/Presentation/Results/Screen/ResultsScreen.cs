using Model;
using Network;
using Presentation.Results.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Results.Screen
{
    public class ResultsScreen : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private ResultsPresenter resultsPresenter;
        
        private PlayerManager myPlayerManager;
        private GameplayManager myGameplayManager;
        private StateManager myStateManager;
        
        public void Initialize(PlayerManager playerManager, GameplayManager gameplayManager, StateManager  stateManager)
        {
            myPlayerManager = playerManager;
            myGameplayManager = gameplayManager;
            myStateManager = stateManager;
        }
        
        private void OnEnable()
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked);
            LevelResult result = myGameplayManager.LatestLevelResult;
            resultsPresenter.Init(result.won, result.energyReward, result.unlockedNewLevel, myPlayerManager.PlayerData.level);
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
