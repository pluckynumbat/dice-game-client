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
    
        public async Task<PlayerData> Send(string sessionID, string playerID, int timeout = 30)
        {
            PlayerData playerData;
        
            string uri = $"{ServerHost}:{ServerPort}{Endpoint}";

            NewPlayerRequestBody requestBody = new NewPlayerRequestBody() {playerID = playerID};
            string postData = JsonUtility.ToJson(requestBody);
        
            UnityWebRequest postRequest = UnityWebRequest.Post(uri,postData, "application/json");
            postRequest.timeout = timeout;
        
            postRequest.SetRequestHeader("Session-Id", sessionID);

            await postRequest.SendWebRequest();
        
            switch (postRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log($"success, new player response: {postRequest.downloadHandler.text}");
                    playerData = JsonUtility.FromJson<PlayerData>(postRequest.downloadHandler.text);
                    break;

                default:
                    Debug.LogError($"failure, reason: {postRequest.error}");
                    playerData = default(PlayerData);
                    break;
            }
        
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