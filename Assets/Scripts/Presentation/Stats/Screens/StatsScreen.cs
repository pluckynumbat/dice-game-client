using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Stats.Screens
{
    public class StatsScreen : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        
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
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveAllListeners();
        }

        private void OnCloseButtonClicked()
        {
            
        }
    }
}