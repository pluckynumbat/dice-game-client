using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using Network;
using Presentation.Gameplay.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Presentation.Gameplay.Screen
{
    public class GameplayScreen : MonoBehaviour
    {
        private const string RollButtonTextNormal = "Roll!";
        private const string RollButtonTextOnRequestFail = "Send Result";
        
        [SerializeField] private Button rollButton;
        [SerializeField] private RollCounterPresenter rollCounterPresenter;
        [SerializeField] private TargetNumberPresenter targetNumberPresenter;
        [SerializeField] private DicePresenter dicePresenter;
        [SerializeField] private TextMeshProUGUI buttonLabelText;

        [SerializeField] private int minimumLevelResultDelayMilliseconds;
        
        private GameplayManager myGameplayManager;
        private StateManager myStateManager;

        private List<int> rolls; // to send to the server in the level result request

        private bool levelResultRequestSent = false;
        
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
            levelResultRequestSent = false;
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
            buttonLabelText.text = RollButtonTextNormal;
        }

        private async void OnRollButtonClicked()
        {
            if (!levelResultRequestSent)
            {
                await PerformDiceRoll(); // normal flow
            }
            else
            {
                await ResendLevelResult(); // post request failure state
            }
        }

        // Roll the die, and if the win condition or loss condition is met, send the level result request to the server
        private async Task PerformDiceRoll()
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
                Task<LevelResultResponse> levelResultTask = myGameplayManager.RequestLevelResult(rolls.ToArray(), new RequestParams() 
                    {Timeout = 10, Retries = 2, DefaultErrorOnFail = ErrorType.CouldNotConnect}); // basic (recoverable) error if the request fails
                
                Task minDelayTask = Task.Delay(minimumLevelResultDelayMilliseconds);  // added to let the player view the dice result before changing game state
                
                await Task.WhenAll(levelResultTask, minDelayTask);

                // the request failed, we will go into an error state here...
                // stay on the screen and let the player press the roll button after some time...
                if (string.IsNullOrEmpty(levelResultTask.Result.playerData.playerID))
                {
                    levelResultRequestSent = true;
                    rollButton.interactable = true;
                    buttonLabelText.text = RollButtonTextOnRequestFail;
                    return;
                }

                // the request succeeded, move on to the level end state and show the result
                myStateManager.ChangeGameState(StateManager.GameState.LevelEnd);
            }
            else
            {
                rollButton.interactable = true;
            }
        }
        
        // the level result request was sent once already, but it failed, so resend it
        private async Task ResendLevelResult()
        {
            rollButton.interactable = false;
            LevelResultResponse levelResultResponse = await myGameplayManager.RequestLevelResult(rolls.ToArray(),  new RequestParams() 
                {Timeout = 10, Retries = 2, DefaultErrorOnFail = ErrorType.CouldNotConnect}); // basic (recoverable) error if the request fails;
            
            if (!string.IsNullOrEmpty(levelResultResponse.playerData.playerID))
            {
                myStateManager.ChangeGameState(StateManager.GameState.LevelEnd);
            }
            else
            {
                rollButton.interactable = true;
            }
        }

        // create and send a level result request that, on failure, leads to a basic (recoverable) error,
        // unless the http status codes are bad request (400) or internal server error (500),
        // in which case, we should go into critical error state
        private Task<LevelResultResponse> SendLevelResultRequest(int[] rollsToSend)
        {
            // create the level result task with the request containing the rolls, and the extra params with the errors
            return myGameplayManager.RequestLevelResult(rollsToSend,
                new RequestParams()
                {
                    Timeout = 10, Retries = 2, DefaultErrorOnFail = ErrorType.CouldNotConnect,
                    CustomHttpStatusBasedErrors = new Dictionary<HttpStatusCode, ErrorType>()
                    {
                        [HttpStatusCode.BadRequest] = ErrorType.CriticalError,
                        [HttpStatusCode.InternalServerError] = ErrorType.CriticalError,
                    }
                }
            );
        }
    }
}
