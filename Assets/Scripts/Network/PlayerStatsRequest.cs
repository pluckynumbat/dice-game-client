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