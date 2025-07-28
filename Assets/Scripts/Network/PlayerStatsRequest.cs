using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    /// The network request to get the player's relevant stats (win count, best score etc...)
    /// </summary>
    public class PlayerStatsRequest
    {
        private const string ServerHost = "http://localhost";
        private const string ServerPort = "8080";
        private const string Endpoint = "/stats/player-stats/";
    
        public async Task<PlayerStats> Send(string sessionID, string playerID, int timeout = 10)
        {
            PlayerStats stats;
        
            string uri = $"{ServerHost}:{ServerPort}{Endpoint}{playerID}";
        
            UnityWebRequest getRequest = UnityWebRequest.Get(uri);
            getRequest.timeout = timeout;
        
            getRequest.SetRequestHeader("Session-Id", sessionID);

            await getRequest.SendWebRequest();
        
            switch (getRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log($"success, player stats response: {getRequest.downloadHandler.text}");
                    stats = JsonUtility.FromJson<PlayerStats>(getRequest.downloadHandler.text);
                    break;

                default:
                    Debug.LogError($"failure, reason: {getRequest.error}");
                    stats = default(PlayerStats);
                    break;
            }
        
            return stats;
        }
    }
    
    [Serializable]
    public struct PlayerStats
    {
        public PlayerLevelStats[] levelStats;
    }

    [Serializable]
    public struct PlayerLevelStats
    {
        public int level;
        public int winCount;
        public int lossCount;
        public int bestScore;
    }
}