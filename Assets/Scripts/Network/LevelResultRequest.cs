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
    
        public async Task<LevelResultResponse> Send(string sessionID, string playerID, int level, int[] rolls, int timeout = 30)
        {
            LevelResultResponse response;
        
            string uri = $"{ServerHost}:{ServerPort}{Endpoint}";

            LevelResultRequestBody requestBody = new LevelResultRequestBody() {playerID = playerID, level = level, rolls = rolls};
            string postData = JsonUtility.ToJson(requestBody);
        
            UnityWebRequest postRequest = UnityWebRequest.Post(uri,postData, "application/json");
            postRequest.timeout = timeout;
        
            postRequest.SetRequestHeader("Session-Id", sessionID);

            await postRequest.SendWebRequest();
        
            switch (postRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log($"success, level result response: {postRequest.downloadHandler.text}");
                    response = JsonUtility.FromJson<LevelResultResponse>(postRequest.downloadHandler.text);
                    break;

                default:
                    Debug.LogError($"failure, reason: {postRequest.error}");
                    response = default(LevelResultResponse);
                    break;
            }
        
            return response;
        }
    }
    
    [Serializable]
    public struct LevelResultRequestBody
    {
        public string playerID;
        public int level;
        public int[] rolls;
    }

    // TODO: maybe get more data back in this, like a rewards struct with level unlocked etc...
    [Serializable]
    public struct LevelResultResponse
    {
        public bool levelWon;
        public PlayerData playerData;
        // TODO: stats will come here
    }
}