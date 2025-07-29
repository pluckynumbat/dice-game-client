using System.Threading;
using System.Threading.Tasks;
using Model;
using Network;
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
        private GameplayManager myGameplayManager;
        private StateManager myStateManager;

        // Properties for the energy display ticker
        private CancellationTokenSource cancelTokenSource;
        private int playerEnergyEstimate;
        private int maxEnergy;
        private int tickPeriodSeconds;
        
        public void Initialize(ConfigManager configManager, PlayerManager playerManager, GameplayManager gameplayManager, StateManager stateManager)
        {
            myConfigManager = configManager;
            myPlayerManager = playerManager;
            myGameplayManager = gameplayManager;
            myStateManager = stateManager;

            maxEnergy = configManager.GameConfig.maxEnergy;
            tickPeriodSeconds = configManager.GameConfig.energyRegenSeconds;
        }

        private void OnEnable()
        {
            statsButton.onClick.AddListener(OnStatsButtonClicked);
            playButton.onClick.AddListener(OnPlayButtonClicked);
            
            playButton.interactable = true;
            
            playerEnergyEstimate = myPlayerManager.PlayerData.energy;
            UpdateDisplay();
            
            cancelTokenSource = new CancellationTokenSource();
            _ = EnergyRegenerationTask(cancelTokenSource.Token);
        }

        private void OnDisable()
        {
            cancelTokenSource.Cancel();
                
            statsButton.onClick.RemoveAllListeners();
            playButton.onClick.RemoveAllListeners(); 
        }

        private void UpdateDisplay()
        {
            playerIdPresenter?.Init(myPlayerManager?.PlayerData.playerID ?? "xxxxxxxx");
            levelSelectionPresenter?.Init(myPlayerManager?.PlayerData.level ?? 1, myPlayerManager?.PlayerData.level ?? 1);
            energyPresenter?.Init(playerEnergyEstimate);
        }

        private void OnStatsButtonClicked()
        {
            myStateManager.ChangeGameState(StateManager.GameState.ViewingStats);
        }
        
        private async void OnPlayButtonClicked()
        {
            int levelToEnter = levelSelectionPresenter.CurrentLevelIndex + 1;
            if (myPlayerManager.PlayerData.level < levelToEnter)
            {
                Debug.Log("cannot enter, level has not been unlocked yet");
                return;
            }

            LevelConfig levelConfig = myConfigManager.GameConfig.levels[levelToEnter - 1];
            if (playerEnergyEstimate < levelConfig.energyCost)
            {
                Debug.Log("cannot enter, our current energy is too low");
                return;
            }

            playButton.interactable = false;
            bool canEnter = await myGameplayManager.RequestLevelEntry(levelToEnter);
            if (canEnter)
            {
                playerEnergyEstimate = myPlayerManager.PlayerData.energy;
                energyPresenter.ConsumeEnergy(levelConfig.energyCost);
                myStateManager.ChangeGameState(StateManager.GameState.LevelInProgress);
            }
            else
            {
                playButton.interactable = true;
            }
        }

       // Ticker that updates the energy display
       private async Task EnergyRegenerationTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(tickPeriodSeconds * 1000, token);
                if (playerEnergyEstimate < maxEnergy)
                {
                    playerEnergyEstimate++;
                    energyPresenter.GainEnergy(1);
                }
            }
            return;
        }
    }
}
