using System.Collections.Generic;
using Model;
using Presentation.Gameplay.Presenters;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Presentation.Gameplay.Screen
{
    public class GameplayScreen : MonoBehaviour
    {
        [SerializeField] private Button rollButton;
        [SerializeField] private RollCounterPresenter rollCounterPresenter;
        [SerializeField] private TargetNumberPresenter targetNumberPresenter;
        [SerializeField] private DicePresenter dicePresenter;

        private GameplayManager myGameplayManager;
        private StateManager myStateManager;

        private List<int> rolls; // to send to the server in the level result request
        
        public void Initialize(GameplayManager gameplayManager, StateManager stateManager)
        {
            myGameplayManager = gameplayManager;
            myStateManager = stateManager;
        }

        private void OnEnable()
        {
            rollButton.onClick.AddListener(OnRollButtonClicked);
            
            rolls = new List<int>();
            rollButton.interactable = true;
            UpdateDisplay(myGameplayManager.CurrentLevelConfig.totalRolls, myGameplayManager.CurrentLevelConfig.target);
        }

        private void OnDisable()
        {
            rollButton.onClick.RemoveAllListeners();
        }

        private void UpdateDisplay(int maxRolls, int targetNumber)
        {
            rollCounterPresenter.Init(maxRolls);
            targetNumberPresenter.Init(targetNumber);
        }

        private async void OnRollButtonClicked()
        {
            rollCounterPresenter.ConsumeRoll();
            
            rollButton.interactable = false;
            await dicePresenter.Roll(Random.Range(0, 6));
            rollButton.interactable = true;

            if (dicePresenter.CurrentDiceNumber == targetNumberPresenter.TargetNumber)
            {
                Debug.Log("WIN");
            }
            else if (rollCounterPresenter.RemainingRolls <= 0)
            {
                Debug.Log("LOSE");
            }
        }
    }
}
