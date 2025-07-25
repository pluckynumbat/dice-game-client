using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    ///  The network request to create a new player 
    /// </summary>
    public class NewPlayerRequest
    {
        private const string ServerHost = "http://localhost";
        private const string ServerPort = "8080";
        private const string Endpoint = "/profile/new-player";
    }
    
    [Serializable]
    public struct NewPlayerRequestBody
    {
        public string playerID;
    }

    [Serializable]
    public struct PlayerData
    {
        public string playerID;
        public int level;
        public int energy;
    }
}