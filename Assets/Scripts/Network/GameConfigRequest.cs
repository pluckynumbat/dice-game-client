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
        private const string Endpoint = "/config/v1/game-config";

        public async Task<GameConfig> Send(int timeout = 15)
        {
            GameConfig gameConfig = null;

            string uri = $"{ServerHost}:{ServerPort}{Endpoint}";

            UnityWebRequest getRequest = UnityWebRequest.Get(uri);
            getRequest.timeout = timeout;

            // TODO: add session id as a custom header here
            
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
                    break;
            }
        
            return gameConfig;
        }
    }

    [Serializable]
    public class GameConfig
    {
        public LevelConfig[] levels;
    }

    [Serializable]
    public class LevelConfig
    {
        public int level;
        public int energyCost;
        public int totalRolls;
        public int target;
        public int energyRewards;
    }
}