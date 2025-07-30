using Model;
using Presentation.Stats.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Stats.Screens
{
    public class StatsScreen : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private StatsPresenter statsPresenter;
        
        private PlayerManager myPlayerManager;
        private StateManager myStateManager;
        
        public void Initialize(PlayerManager playerManager, StateManager stateManager)
        {
            myPlayerManager = playerManager;
            myStateManager = stateManager;
        }
        
        private void OnEnable()
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
            UpdateDisplay();
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveAllListeners();
        }
        
        private void UpdateDisplay()
        {
            statsPresenter?.Init(myPlayerManager.Stats.levelStats);
        }

        private void OnCloseButtonClicked()
        {
            myStateManager.ChangeGameState(StateManager.GameState.MainMenu);
        }
    }
}