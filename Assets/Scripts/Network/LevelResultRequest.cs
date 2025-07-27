using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    ///  The network request to get the result of the dice game level, and updated player data (and stats) along with it
    /// </summary>
    public class LevelResultRequest
    {
        private const string ServerHost = "http://localhost";
        private const string ServerPort = "8080";
        private const string Endpoint = "/gameplay/result";
    }
    
    [Serializable]
    public struct LevelResultRequestBody
    {
        public string playerID;
        public int level;
        public int[] rolls;
    }

    [Serializable]
    public struct LevelResultResponse
    {
        public bool levelWon;
        public PlayerData playerData;
        // TODO: stats will come here
    }
}