using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    ///  The network request to attempt to enter a dice game level
    /// </summary>
    public class EnterLevelRequest
    {
        private const string ServerHost = "http://localhost";
        private const string ServerPort = "8080";
        private const string Endpoint = "/gameplay/entry";
    }
    
    [Serializable]
    public struct EnterLevelRequestBody
    {
        public string playerID;
        public int level;
    }

    [Serializable]
    public struct EnterLevelResponse
    {
        public bool accessGranted;
        public PlayerData playerData;
    }
}