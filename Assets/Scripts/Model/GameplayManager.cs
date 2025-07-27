using System.Threading.Tasks;
using Network;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// Gameplay manager will handle all data and functionality related to playing the dice game levels
    /// </summary>
    public class GameplayManager
    {
        public int CurrentLevel;
        public LevelConfig CurrentLevelConfig;
        public bool WonLastPlayedLevel;
        
        public async Task<bool> RequestLevelEntry(int level)
        {
            EnterLevelRequest request = new EnterLevelRequest();
            
            EnterLevelResponse responseData = await request.Send(
                GameRoot.Instance.AuthManager.SessionID, GameRoot.Instance.AuthManager.PlayerID, level);

            if (string.IsNullOrEmpty(responseData.playerData.playerID))
            {
                Debug.LogError("enter level request failed :(");
                return false;
            }

            // request was successful (regardless of whether access was granted or not)
            bool accessGranted = responseData.accessGranted;
            GameRoot.Instance.PlayerManager.UpdatePlayerData(responseData.playerData);
            if (accessGranted)
            {
                CurrentLevel = level;
                CurrentLevelConfig = GameRoot.Instance.ConfigManager.GameConfig.levels[level - 1];
            }

            return accessGranted;
        }
        
        public async Task<bool> RequestLevelResult(int[] rolls)
        {
            LevelResultRequest request = new LevelResultRequest();
            
            LevelResultResponse responseData = await request.Send(GameRoot.Instance.AuthManager.SessionID, GameRoot.Instance.AuthManager.PlayerID, CurrentLevel, rolls);

            if (string.IsNullOrEmpty(responseData.playerData.playerID))
            {
                Debug.LogError("level result request failed :(");
                return false;
            }

            // request was successful (regardless of whether player won or not)
            bool levelWon = responseData.levelWon;
            GameRoot.Instance.PlayerManager.UpdatePlayerData(responseData.playerData);
            WonLastPlayedLevel = levelWon;
            
            // reset current level and level config
            CurrentLevel = 0;
            CurrentLevelConfig = default(LevelConfig);
            
            return levelWon;
        }
    }
}
