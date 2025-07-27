using Presentation.Main.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Main.Screen
{
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private Button statsButton;
        [SerializeField] private Button playButton;
        [SerializeField] private LevelSelectionPresenter levelSelectionPresenter;
        [SerializeField] private EnergyPresenter energyPresenter;
        [SerializeField] private PlayerIdPresenter playerIdPresenter;

        private ConfigManager myConfigManager;
        private PlayerManager myPlayerManager;

        public void Initialize(ConfigManager configManager, PlayerManager playerManager)
        {
            myConfigManager = configManager;
            myPlayerManager = playerManager;
        }

        private void OnEnable()
        {
            statsButton.onClick.AddListener(OnStatsButtonClicked);
            playButton.onClick.AddListener(OnPlayButtonClicked);
            
            UpdateDisplay();
        }

        private void OnDisable()
        {
            statsButton.onClick.RemoveAllListeners();
            playButton.onClick.RemoveAllListeners(); 
        }

        private void UpdateDisplay()
        {
            playerIdPresenter?.Init(myPlayerManager?.PlayerData.playerID ?? "xxxxxxxx");
            levelSelectionPresenter?.Init(myConfigManager?.GameConfig.levels.Length ?? 1, myPlayerManager?.PlayerData.level ?? 1);
            energyPresenter?.Init(playerEnergyEstimate);
        }

        private void OnStatsButtonClicked()
        {
            
        }
        
        private void OnPlayButtonClicked()
        {
            // levelSelectionPresenter.CurrentLevelIndex
        }
    }
}
