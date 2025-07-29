using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    /// The network request to get the game config
    /// </summary>
    public class GameConfigRequest
    {
        private const string ServerHost = "http://localhost";
        private const string ServerPort = "8080";
        private const string Endpoint = "/config/game-config";

        public async Task<GameConfig> Send(string sessionID, int timeout = 15)
        {
            GameConfig gameConfig;

            string uri = $"{ServerHost}:{ServerPort}{Endpoint}";

            UnityWebRequest getRequest = UnityWebRequest.Get(uri);
            getRequest.timeout = timeout;

            getRequest.SetRequestHeader("Session-Id", sessionID);
            
            // TODO: add more error / exception handling in the following code?

            await getRequest.SendWebRequest();
        
            switch (getRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log($"success, game config response: {getRequest.downloadHandler.text}");
                    gameConfig = JsonUtility.FromJson<GameConfig>(getRequest.downloadHandler.text);
                    break;

                default:
                    Debug.LogError($"failure, reason: {getRequest.error}");
                    gameConfig = default(GameConfig);
                    break;
            }
        
            return gameConfig;
        }
    }

    [Serializable]
    public struct GameConfig
    {
        public LevelConfig[] levels;
        public int defaultLevel;
        public int maxEnergy;
        public int energyRegenSeconds;
        public int defaultLevelScore;
    }

    [Serializable]
    public struct LevelConfig
    {
        public int level;
        public int energyCost;
        public int totalRolls;
        public int target;
        public int energyRewards;
    }
}