using System;
using System.Threading.Tasks;
using Model;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    /// The network request to get the player's relevant stats (win count, best score etc...)
    /// </summary>
    public class PlayerStatsRequest
    {
        private const string Endpoint = "/stats/player-stats/";
    
        public async Task<PlayerStatsResponse> Send(string playerID, RequestParams extraParams)
        {
            PlayerStatsResponse playerStatsResponse =  await 
                GameRoot.Instance.NetRequestManager.SendGetRequest<PlayerStatsResponse>(
                $"{Endpoint}{playerID}", extraParams);
            return playerStatsResponse;
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
    
    [Serializable]
    public struct PlayerStatsResponse
    {
        public string playerID;
        public PlayerStats playerStats;
    }
}