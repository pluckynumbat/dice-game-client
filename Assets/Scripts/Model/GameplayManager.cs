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
        public LevelResult LatestLevelResult;
        public async Task<bool> RequestLevelEntry(int level, RequestParams extraParams)
        {
            EnterLevelRequest request = new EnterLevelRequest();
            
            EnterLevelResponse responseData = await request.Send(GameRoot.Instance.AuthManager.PlayerID, level, extraParams);
            
            if (string.IsNullOrEmpty(responseData.playerData.playerID))
            {
                Debug.LogWarning("enter level request failed :(");
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
        
        public async Task<LevelResultResponse> RequestLevelResult(int[] rolls, RequestParams extraParams)
        {
            LevelResultRequest request = new LevelResultRequest();
            
            LevelResultResponse responseData = await request.Send(GameRoot.Instance.AuthManager.PlayerID, CurrentLevel, rolls, extraParams);

            if (string.IsNullOrEmpty(responseData.playerData.playerID))
            {
                Debug.LogError("level result request failed :(");
                return default(LevelResultResponse);
            }

            // request was successful (regardless of whether player won or not)
            LatestLevelResult = responseData.levelResult;
            GameRoot.Instance.PlayerManager.UpdatePlayerData(responseData.playerData);
            GameRoot.Instance.PlayerManager.UpdateStats(responseData.statsData);
            
            // reset current level and level config
            CurrentLevel = 0;
            CurrentLevelConfig = default(LevelConfig);
            
            return responseData;
        }
    }
}
