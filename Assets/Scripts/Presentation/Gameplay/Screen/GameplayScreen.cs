using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using Network;
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

        [SerializeField] private int minimumLevelResultDelayMilliseconds;
        
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
            
            int diceRollValue = Random.Range(1, 7);
            rolls.Add(diceRollValue);
            await dicePresenter.Roll(diceRollValue - 1);

            bool win = (dicePresenter.CurrentDiceNumber == targetNumberPresenter.TargetNumber); // win condition!
            bool lose = (!win) && (rollCounterPresenter.RemainingRolls <= 0); // lose condition :(

            if (win || lose)
            {
                Task<LevelResult> levelResultTask = myGameplayManager.RequestLevelResult(rolls.ToArray());
                Task minDelayTask = Task.Delay(minimumLevelResultDelayMilliseconds);  // added to let the player view the dice result before changing game state
                
                await Task.WhenAll(levelResultTask, minDelayTask);
                
                Debug.Log("level " + (levelResultTask.Result.won ? "won" : "lost"));
                myStateManager.ChangeGameState(StateManager.GameState.LevelEnd);
            }
            else
            {
                rollButton.interactable = true;
            }
        }
    }
}
