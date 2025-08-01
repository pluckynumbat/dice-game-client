using System;
using System.Threading.Tasks;
using Model;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    /// <summary>
    ///  The network request to create a new player 
    /// </summary>
    public class NewPlayerRequest
    {
        private const string Endpoint = "/profile/new-player";
    
        public async Task<PlayerData> Send(string playerID, RequestParams extraParams)
        {
            PlayerData playerData = await GameRoot.Instance.NetRequestManager.SendPostRequest
                <PlayerData, NewPlayerRequestBody>(Endpoint, new NewPlayerRequestBody() {playerID = playerID}, extraParams);
            
            return playerData;
        }
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